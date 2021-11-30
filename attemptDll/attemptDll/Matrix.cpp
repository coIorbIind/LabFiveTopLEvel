#include "pch.h"
#include "Matrix.h"

Matrix::Matrix(int n) {
	for (int i = 0; i < n; i++)
	{
		this->row.push_back(2 * i);
		this->col.push_back(3 * i);
	}
	col[0] = row[row.size() - 1];
}

Matrix::Matrix(const vector<double>& row, const vector<double>& col) {
	if (row.size() != col.size())
		throw "Строка и столбец имею разные размеры";
	if (row[row.size() - 1] != col[0])
		throw "Последний элемент строки должен совпадать с первым элементом столбца";
	(this->row).clear();
	(this->col).clear();
	for (int i = 0; i < row.size(); i++)
	{
		this->row.push_back(row[i]);
		this->col.push_back(col[i]);
	}

}

Matrix::Matrix(const Matrix& other) {
	this->row.clear();
	this->col.clear();
	for (int i = 0; i < row.size(); i++)
	{
		this->row.push_back(other.row[i]);
		this->col.push_back(other.col[i]);
	}
}

Matrix& Matrix::operator= (const Matrix& other) {
	this->row.clear();
	this->col.clear();
	for (int i = 0; i < row.size(); i++)
	{
		this->row.push_back(other.row[i]);
		this->col.push_back(other.col[i]);
	}
	return *this;
}

double Matrix::check() {
	double total = 0;
	for (int i = 0; i < col.size(); i++)
	{
		total += col[i];
	}
	return total;
}

vector<double> convert(int n, double arr[]) {
	vector<double> temp;
	for (int i = 0; i < n; i++)
	{
		temp.push_back(arr[i]);
	}
	return temp;
}

void Matrix::solve(vector<double>& solution, const vector<double>& free_col)
{
	int n = row.size();
	vector<double> x(n);
	vector<double> y(n);

	x[0] = 1 / col[0];
	y[0] = 1 / col[0];
	double F, G, r, s, t;
	for (int k = 1; k < n; k++)
	{
		F = G = 0.0;
		for (int i = 0; i < k; i++)
		{
			F += row[(n - 1) - (k - i)] * x[i];
			G += col[i + 1] * y[i];
		}
		r = 1.0 / (1.0 - F * G);
		s = -r * F;
		t = -r * G;

		vector<double> tempY(y);
		y[0] = t * x[0];
		x[0] = r * x[0];
		for (int i = 1; i <= k; i++)
		{
			y[i] = t * x[i] + r * tempY[i - 1];
			x[i] = r * x[i] + s * tempY[i - 1];
		}
	}

	for (int i = 0; i < n; i++)
	{
		vector<double> transpose(n);
		for (int j = 0; j < n; j++)
		{
			double a = 0.0;
			for (int k = 0; k < min(i, j) + 1; k++)
				a += x[i - k] * y[n - 1 - j + k];

			double b = 0.0;
			if (i != 0 && j != 0)
				for (int k = 0; k < min(i, j); k++)
					b += y[i - 1 - k] * x[n - j + k];

			transpose[j] = a - b;
		}

		solution[i] = 0.0;
		for (int k = 0; k < n; k++)
			solution[i] += transpose[k] / x[0] * free_col[(n - 1) - k];
	}
}