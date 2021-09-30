//: [Previous](@previous)
/*:
 # Async Sequences
 SE-0298 introduces the ability to loop over asynchronous sequences of values using a new AsyncSequence protocol
 
 Using AsyncSequence is almost identical to using Sequence, with the exception that your types should conform to AsyncSequence and AsyncIterator, and your next() method should be marked async. When it comes time for your sequence to end, make sure you send back nil from next(), just as with Sequence.
 */
import UIKit

public struct RandomDogGenerator: AsyncSequence {
    
    public typealias Element = DogResponse
    
    let max: Int
    
    public struct AsyncIterator: AsyncIteratorProtocol {
        var current = 0
        let max: Int
        
        mutating public func next() async -> DogResponse? {
            if current < max {
                current += 1
                return try? await fetchRandomDog()
            } else {
                return nil
            }
        }
    }
    
    public func makeAsyncIterator() -> AsyncIterator {
        AsyncIterator(max: max)
    }
}

/*:
 ### use async sequence
 */

func printAllRandomDogs() async {
    for await dog in RandomDogGenerator(max: 5) {
        print("Dog \(dog.message)")
    }
}

Task {
    await printAllRandomDogs()
}

//: [Next](@next)
