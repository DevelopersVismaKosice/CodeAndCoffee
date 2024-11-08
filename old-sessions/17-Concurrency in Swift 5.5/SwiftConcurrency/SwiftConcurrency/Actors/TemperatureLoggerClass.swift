//
//  TemperatureLoggerClass.swift
//  SwiftConcurrency
//
//  Created by Michal Melich on 28/09/2021.
//

import Foundation

class TemperatureLoggerClass {
    let label: String
    var measurements: [Int]
    private(set) var max: Int
    
    init(label: String, measurement: Int) {
        self.label = label
        self.measurements = [measurement]
        self.max = measurement
    }
}

extension TemperatureLoggerClass {
    func update(with measurement: Int) {
        measurements.append(measurement)
        if measurement > max {
            max = measurement
        }
    }
}
