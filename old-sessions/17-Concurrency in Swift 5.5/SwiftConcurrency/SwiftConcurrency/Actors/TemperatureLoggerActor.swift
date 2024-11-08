//
//  TemperatureLogger.swift
//  SwiftConcurrency
//
//  Created by Michal Melich on 28/09/2021.
//

import Foundation

actor TemperatureLoggerActor {
    let label: String
    var measurements: [Int]
    private(set) var max: Int
    
    init(label: String, measurement: Int) {
        self.label = label
        self.measurements = [measurement]
        self.max = measurement
    }
}

extension TemperatureLoggerActor {
    func update(with measurement: Int) {
        measurements.append(measurement)
        if measurement > max {
            max = measurement
        }
    }
}
