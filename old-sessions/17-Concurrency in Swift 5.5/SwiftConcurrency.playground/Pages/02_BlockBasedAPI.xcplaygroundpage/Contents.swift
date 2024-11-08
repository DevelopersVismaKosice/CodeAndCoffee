//: [Previous](@previous)
/*:
 ## Block Based API
 */

import UIKit

fetchRandomDog(completion: { result in
    if case .success(let dog) = result {
        fetchDogImageData(dog, completion: { data in
            imageFromData(data ?? Data()) { image in
                let dogImage = image
                print("Dog image: \(String(describing: dogImage))")
            }
        })
    }
})


/*:
 ## DispatchQueue
 */

func codeAndCoffee() {
    print(String(format: "%02X", 49374))
    print("&")
    print(String(format: "%02X", 12648430))
}

let customQueue = DispatchQueue(label: "Code&CoffeeQueue")
customQueue.async {
    codeAndCoffee()
}

DispatchQueue.main.async {
    codeAndCoffee()
}
print("async")

customQueue.sync {
    codeAndCoffee()
}
print("sync")

DispatchQueue.global(qos: .background).async {
    codeAndCoffee()
}

/*:
 ## Operations
 */

let queue = OperationQueue()

queue.addOperation {
    codeAndCoffee()
}

class MakeCoffee: Operation {
    override func main() {
        print("""
                  )  (
                 (   ) )
                  ) ( (
                _______)_
             .-'---------|
            ( C|-=-=-=-=-|
             '-.-=-=-=-=-|
               '_________'
                '-------'
        """)
    }
}

class MakeTea: Operation {
    
    let type: String
    
    override var isAsynchronous: Bool { return true }
    override var isExecuting: Bool { return _executing }
    override var isFinished: Bool { return _finished }
    
    init(type: String) {
        self.type = type
    }
    
    var _executing: Bool = false {
        willSet {
            willChangeValue(for: \.isExecuting)
        }
        didSet {
            didChangeValue(for: \.isExecuting)
        }
    }
    
    var _finished: Bool = false {
        willSet {
            willChangeValue(for: \.isFinished)
        }
        didSet {
            didChangeValue(for: \.isFinished)
        }
    }
    
    override func start() {
        _executing = true
        _finished = false
        DispatchQueue.global(qos: .background).async {
            print("\(self.type) tea\n" +
            """
                             ;,'
                     _o_    ;:;'
                 ,-.'---`.__ ;
                ((j`=====',-'
                 `-\\     /
                    `-=-'
            """)
            
            self._executing = false
            self._finished = true
        }
    }
}

MakeCoffee().start()

queue.addOperation(MakeCoffee())
queue.addOperation(MakeTea(type: "black"))


let hotTea = MakeTea(type: "hot")
let blackTea = MakeTea(type: "black")

queue.addOperation(hotTea)
queue.addOperation(blackTea)

let whiteTea = MakeTea(type: "white")
let greenTea = MakeTea(type: "green")

greenTea.addDependency(whiteTea)

queue.addOperation(greenTea)

queue.addOperation(whiteTea)


//: [Next](@next)
