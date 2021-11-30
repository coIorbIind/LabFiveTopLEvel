#pragma once
#include <iostream>
#include <vector>
#include <string>

using namespace std;

class Matrix {
	vector<double> row;
	vector<double> col;

public:
	Matrix(int n);
	Matrix(const vector<double>& row, const vector<double>& col);
	Matrix(const Matrix& other);
	Matrix& operator= (const Matrix& other);
	void solve(vector<double>& solution, const vector<double>& free_col);
	double check();
};

vector<double> convert(int n, double arr[]);

extern "C" __declspec(dllexport) double solve_default(int n, int count) {
	Matrix matrix = Matrix(n);

	vector<double> free_col;
	for (int i = 0; i < n; i++)
		free_col.push_back(i);

	vector<double> solution;
	for (int i = 0; i < n; i++)
		solution.push_back(i);

	clock_t start = clock();

	for (int i = 0; i < count; i++)
	{
		matrix.solve(solution, free_col);
	}

	return (clock() - start) / (double)CLOCKS_PER_SEC;
}

extern "C" __declspec(dllexport) void solve_matrix(int n, double row[], double col[], double free_col[], double solution[]) {

	Matrix matrix = Matrix(convert(n, row), convert(n, col));
	vector<double> free_col_vec = convert(n, free_col);
	vector<double> sol_vec = convert(n, solution);
	matrix.solve(sol_vec, free_col_vec);

	for (int i = 0; i < n; i++)
	{
		solution[i] = sol_vec[i];
	}

}