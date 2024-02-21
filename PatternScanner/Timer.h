#pragma once
#include <chrono>
#include <iostream>

class Timer {
 public:
  Timer() : _timer_name(nullptr) {
    Start();
  }

  Timer(const char* timer_name) : _timer_name(timer_name) {
    Start();
  }

  ~Timer() {
    Stop();
  }

  void Start() {
    m_StartTimepoint = std::chrono::high_resolution_clock::now();
  }

  void Stop() {
    auto endTimepoint = std::chrono::high_resolution_clock::now();

    auto start = std::chrono::time_point_cast<std::chrono::microseconds>(m_StartTimepoint).time_since_epoch().count();
    auto end = std::chrono::time_point_cast<std::chrono::microseconds>(endTimepoint).time_since_epoch().count();

    auto duration = end - start;
    double ms = duration * 0.001;

    //std::cout << duration << "us (" << ms << "ms)\n";
    if (_timer_name != nullptr) {
      std::cout << _timer_name << " (" << ms << "ms)\n ";
    } else {
      std::cout << "(" << ms << "ms)\n ";
    }
  }

 private:
  const char* _timer_name;
  std::chrono::time_point<std::chrono::high_resolution_clock> m_StartTimepoint;
};
