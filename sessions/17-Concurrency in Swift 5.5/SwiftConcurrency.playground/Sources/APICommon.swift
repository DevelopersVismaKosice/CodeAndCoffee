import Foundation
import UIKit

public protocol Request {
    associatedtype Response: Decodable
    
    var url: String { get }
}

public enum ResponseError: Error {
    case NoData
}

public struct DogResponse: Codable {
    public let message: String
    public let status: String
}

public struct RandomDogRequest: Request {
    public typealias Response = DogResponse
    public let url: String = "https://dog.ceo/api/breeds/image/random"
}

public struct URLDataRequest: Request {
    public typealias Response = Data
    public let url: String
}


public func imageFromData(_ data: Data, completion: @escaping (UIImage?) -> Void) {
    DispatchQueue.main.async {
        completion(UIImage(data: data))
    }
}

public func imageFromData(_ data: Data) async -> UIImage? {
    return UIImage(data: data)
}


