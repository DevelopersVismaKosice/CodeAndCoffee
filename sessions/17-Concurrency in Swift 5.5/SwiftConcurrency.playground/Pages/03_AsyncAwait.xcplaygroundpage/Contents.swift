//: [Previous](@previous)
/*:
 # Async/await
 SE-0296 introduces asynchronous (async) functions into Swift
 */
import UIKit

Task {
    let dog = try await fetchRandomDog()
    let dogImageData = try await fetchDogImageData(dog)
    let dogImage = await imageFromData(dogImageData)
}


//: [Next](@next)
