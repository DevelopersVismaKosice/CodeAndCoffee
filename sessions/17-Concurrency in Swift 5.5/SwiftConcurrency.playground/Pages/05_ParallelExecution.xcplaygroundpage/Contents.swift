//: [Previous](@previous)

import UIKit
import PlaygroundSupport

PlaygroundPage.current.needsIndefiniteExecution = true

/*:
 ### Serial execution
 */

Task {
    let first = try await fetchRandomDogWithSleep(label: "1")
    let second = try await fetchRandomDogWithSleep(label: "2")
    let third = try await fetchRandomDogWithSleep(label: "3")

    let dogs = [first, second, third]
    print("dogs \(dogs)")
}

/*:
 ### Parallel execution
 doesn't run in playgrounds on Mac OS 11 Big Sur,
 run in simulator or MacOS 12 Monterey
 */

//Task {
//    async let first = fetchRandomDogWithSleep()
//    async let second = fetchRandomDogWithSleep()
//    async let third = fetchRandomDogWithSleep()
//
//    print("waiting for...")
//    let dogs = await [first, second, third]
//    print("dogs \(dogs)")
//}


//: [Next](@next)
