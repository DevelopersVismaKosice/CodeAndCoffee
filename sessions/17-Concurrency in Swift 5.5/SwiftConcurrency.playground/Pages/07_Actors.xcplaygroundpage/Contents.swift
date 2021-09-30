//: [Previous](@previous)
/*:
 # Actors
 
 SE-0306 introduces actors, which are conceptually similar to classes that are safe to use in concurrent environments. This is possible because Swift ensures that mutable state inside your actor is only ever accessed by a single thread at any given time, which helps eliminate a variety of serious bugs right at the compiler level.
 */

import UIKit

actor TemperatureLogger {
    let label: String
    var measurements: [Int]
    private(set) var max: Int
    
    init(label: String, measurement: Int) {
        self.label = label
        self.measurements = [measurement]
        self.max = measurement
    }
}

extension TemperatureLogger {
    func update(with measurement: Int) {
        measurements.append(measurement)
        if measurement > max {
            max = measurement
        }
    }
}

Task {
    let logger = TemperatureLogger(label: "Outdoors", measurement: 25)
    print(await logger.max)
}

Task {
    let logger = TemperatureLogger(label: "Outdoors", measurement: 25)
    var max = await logger.max
    print("max temp \(max)")
    await logger.update(with: 26)
    max = await logger.max
    print("max temp \(max)")
}

//: [Next](@next)

