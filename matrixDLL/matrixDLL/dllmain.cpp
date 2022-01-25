// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"

#include <iostream>
#include <cstdio>
#include <ctime>


class Matrix
{
	int n = 0;
	double* s;

public:
	Matrix(int n)
	{
		this->n = n;
		s = new double[n];
		for (int i = 0; i < n; i++)
			s[i] = 2 * i + 1;
	}

	Matrix(int n, double* mas)
	{
		this->n = n;
		s = new double[n];
		for (int i = 0; i < n; i++)
			s[i] = mas[i];
	}

	Matrix(const Matrix& ob)
	{
		n = ob.n;
		s = new double[n];
		for (int i = 0; i < n; i++)
			s[i] = ob.s[i];
	}

	~Matrix()
	{
		delete[] s;
	}

	Matrix& operator=(const Matrix& temp)
	{
		if (this == &temp) return *this;
		delete[] s;
		this->n = temp.n;
		s = new double[n];
		for (int i = 0; i < n; i++)
			s[i] = temp.s[i];
		return *this;
	}

	double* Solve(double b[], double* rez = nullptr)
	{
		double* x = new double[n] {};
		double* y = new double[n] {};
		double** mat1 = new double* [n] {};
		double** mat2 = new double* [n] {};
		double** mat3 = new double* [n] {};
		double** mat4 = new double* [n] {};
		double** mat12 = new double* [n] {};
		double** mat34 = new double* [n] {};
		x[0] = 1.0 / s[0];
		y[0] = x[0];
		for (int k = 1; k < n; k++)
		{
			double Fk = 0;
			double Gk = 0;
			double* xk = new double[k] {};
			double* yk = new double[k] {};
			for (int i = 0; i < k; i++)
			{
				Fk += s[k - i] * x[i];
				Gk += s[i + 1] * y[i];
			}
			double rk = 1.0 / (1.0 - Fk * Gk);
			double sk = -rk * Fk;
			double tk = -rk * Gk;

			for (int i = 0; i < k; i++)
			{
				xk[i] = x[i];
				yk[i] = y[i];
			}
			x[0] = xk[0] * rk + 0 * sk;
			y[0] = xk[0] * tk + 0 * rk;
			for (int i = 1; i < k; i++)
			{
				x[i] = xk[i] * rk + yk[i - 1] * sk;
				y[i] = xk[i] * tk + yk[i - 1] * rk;
			}
			x[k] = 0 * rk + yk[k - 1] * sk;
			y[k] = 0 * tk + yk[k - 1] * rk;
			delete[] xk; delete[] yk;
		}

		for (int i = 0; i < n; i++)
		{
			mat1[i] = new double[n] {};
			mat2[i] = new double[n] {};
			mat3[i] = new double[n] {};
			mat4[i] = new double[n] {};
			for (int j = 0; j < n; j++)
			{
				if (i >= j)  mat1[i][j] = x[i - j];
				else mat1[i][j] = 0;
				if (j >= i) mat2[i][j] = y[n - 1 - j + i];
				else mat2[i][j] = 0;
				if (i > j)
				{
					mat3[i][j] = y[i - 1 - j];
					mat4[i][j] = 0;
				}
				else
				{
					mat3[i][j] = 0;
					mat4[i][j] = x[n - 1 - j + 1 + i];
				}
			}
		}

		for (int i = 0; i < n; i++)
		{
			mat12[i] = new double[n] {};
			mat34[i] = new double[n] {};
			for (int j = 0; j < n; j++)
			{

				for (int k = 0; k < n; k++)
				{
					mat12[i][j] += mat1[i][k] * mat2[k][j];
					mat34[i][j] += mat3[i][k] * mat4[k][j];
				}

			}
		}

		double** invert = new double* [n] {};
		double rev_x = 1.0 / x[0];
		for (int i = 0; i < n; i++)
		{
			invert[i] = new double[n] {};
			for (int j = 0; j < n; j++)
				invert[i][j] = rev_x * (mat12[i][j] - mat34[i][j]);
		}

		if (rez == nullptr)  rez = new double[n] {};
		for (int i = 0; i < n; i++)
		{
			rez[i] = 0;
			for (int j = 0; j < n; j++)
				rez[i] += invert[i][j] * b[j];
		}

		for (int i = 0; i < n; i++)
		{
			delete[] mat1[i];
			delete[] mat2[i];
			delete[] mat3[i];
			delete[] mat4[i];
			delete[] mat12[i];
			delete[] mat34[i];
			delete[]invert[i];
		}
		delete[]x;
		delete[]y;
		delete[] mat1;
		delete[] mat2;
		delete[] mat3;
		delete[] mat4;
		delete[] mat12;
		delete[] mat34;
		delete[] invert;
		return rez;
	}
};



extern "C" {
	__declspec(dllexport) double Time(int n, int repeat)
	{
		double time = 0;
		std::clock_t begin = std::clock();
		Matrix mat(n);
		double* b = new double[n] {};
		for (int i = 0; i < n; i++)
			b[i] = i * i;
		for (int i = 0; i < repeat; i++)
			mat.Solve(b);
		time = (std::clock() - begin) / (double)CLOCKS_PER_SEC;
		delete[] b;
		return time;
	}



	__declspec(dllexport) void Solve(int n, double max_1[], double max_2[], double* max_3)
	{
		Matrix matrix = Matrix(n, max_1);
		matrix.Solve(max_2, max_3);
	}
}