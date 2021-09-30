//
//  Tasks.swift
//  SwiftConcurrency
//
//  Created by Michal Melich on 28/09/2021.
//

import Foundation

class Tasks {
    static func performParallelTasks() {
        Task {
            async let first = fetchRandomDogWithSleep(label: "1")
            async let second = fetchRandomDogWithSleep(label: "2")
            async let third = fetchRandomDogWithSleep(label: "3")
            
            print("waiting for...")
            let dogs = await [first, second, third]
            print("dogs \(dogs)")
        }
    }
    
    static func performActorTask() {
        Task {
            let logger = TemperatureLoggerActor(label: "Outdoors", measurement: 25)
            
            await withTaskGroup(of: Void.self) { group in
                
                for i in 1...1000 {
                    group.addTask {
                        let measurements = await logger.measurements
                        let max = await logger.max
                        print("\(i).1: measurements \(measurements) max: \(max)")
                        
                        return Void()
                    }
                    
                    group.addTask {
                        await logger.update(with: await logger.max + 1)
                        return Void()
                    }
                }
            }
        }
    }
    
    static var logger = TemperatureLoggerClass(label: "Outdoor", measurement: 25)
    static var queue = OperationQueue()
    static var updateQueue = OperationQueue()
    
    static func performClassTask() {
        
        logger = TemperatureLoggerClass(label: "Outdoor", measurement: 25)
        
        for i in 1...1000 {
            queue.addOperation {
                let measurements = logger.measurements
                let max = logger.max
                print("\(i).1: measurements \(measurements) max: \(max)")
            }
            
            updateQueue.addOperation {
                logger.update(with: logger.max + 1)
            }
        }
    }
}

