//: [Previous](@previous)
/*:
 # Tasks and Task Groups
 
 A task is a unit of work that can be run asynchronously as part of your program. All asynchronous code runs as part of some task. The async-let syntax described in the previous section creates a child task for you. You can also create a task group and add child tasks to that group, which gives you more control over priority and cancellation, and lets you create a dynamic number of tasks.

 Tasks are arranged in a hierarchy. Each task in a task group has the same parent task, and each task can have child tasks. Because of the explicit relationship between tasks and task groups, this approach is called structured concurrency. Although you take on some of the responsibility for correctness, the explicit parent-child relationships between tasks lets Swift handle some behaviors like propagating cancellation for you, and lets Swift detect some errors at compile time.
 */
import UIKit

func fibonacci(of n: Int) -> Int {
    
    if n <= 1 {
        return n
    }

    return fibonacci(of: n-1) + fibonacci(of: n-2)
}

func fibonacci2(of n: Int) -> Int {
    var first = 0
    var second = 1

    for _ in 0..<n {
        let previous = first
        first = second
        second = previous + first
    }
    
    return first
}

//fibonacci(of: 10)
//fibonacci(of: 50)
func printFibonaciiSequence() async {
    let task = Task { () -> [Int] in
        return (0..<50).map(fibonacci2(of:))
    }
    print("The first 50 numbers in the Fibonacci sequence are: \(await task.value)")
}

Task { await printFibonaciiSequence() }


// cancellation
func cancelSleepingTask() async {
    let task = Task { () -> String in
        print("Start")
        try await Task.sleep(nanoseconds: 1_000_000_000)
        try Task.checkCancellation()
        return "Done"
    }
    
    task.cancel()
    
    do {
        let result = try await task.value
        print("Result: \(result)")
    } catch {
        print("Task was cancelled with error \(error)")
    }
}

Task {
    await cancelSleepingTask()
}


// task groups

Task {
    
    let results = try await withThrowingTaskGroup(of: Data.self, returning: [Data].self, body: { taskGroup in
      
        let dogs = [try await fetchRandomDog(), try await fetchRandomDog(), try await fetchRandomDog()]
        for dog in dogs {
            taskGroup.addTask { try await fetchDogImageData(dog) }
        }
        
        var images: [Data] = []
        for try await result in taskGroup {
            print("result \(result)")
            images.append(result)
        }
        return []
    })
    
    print("Random dogs \(results)")
}


//: [Next](@next)
