#include <iostream>
#include <vector>
#include <omp.h>

void quicksort(std::vector<int>& arr, int left, int right) {
    if (left >= right)
        return;

    int pivot = arr[left + (right - left) / 2];
    int i = left, j = right;

    while (i <= j) {
        while (arr[i] < pivot)
            i++;
        while (arr[j] > pivot)
            j--;
        if (i <= j) {
            std::swap(arr[i], arr[j]);
            i++;
            j--;
        }
    }

#pragma omp parallel sections
    {
#pragma omp section
        {
            quicksort(arr, left, j);
        }

#pragma omp section
        {
            quicksort(arr, i, right);
        }
    }
}

int main() {
    std::vector<int> arr = { 9, 4, 2, 7, 5, 1, 8, 3, 6 };
    int numThreads = 4;

    omp_set_num_threads(numThreads);

    std::cout << "Original array: ";
    for (int num : arr)
        std::cout << num << " ";
    std::cout << std::endl;

    quicksort(arr, 0, arr.size() - 1);

    std::cout << "Sorted array: ";
    for (int num : arr)
        std::cout << num << " ";
    std::cout << std::endl;

    return 0;
}