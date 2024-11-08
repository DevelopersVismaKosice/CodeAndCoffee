//
//  Concurrency.swift
//  SwiftConcurrency
//
//  Created by Michal Melich on 22/09/2021.
//

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

public func fetchJSON<R: Request>(_ request: R) async -> Result<R.Response, Error> {
    let url = URL(string: request.url)!
    do {
        let (data, _) = try await URLSession.shared.data(from: url)
        do {
            let decoder = JSONDecoder()
            let result = try decoder.decode(R.Response.self, from: data)
            return .success(result)
        } catch {
            return .failure(error)
        }
    
    } catch {
        return .failure(error)
    }
}

public func fetchData<R: Request>(_ request: R) async -> Result<R.Response, Error> where R.Response == Data {
    let url = URL(string: request.url)!

    
    do {
        let (data, _) = try await URLSession.shared.data(from: url)
        return .success(data)
    } catch {
        return .failure(error)
    }
}

public func fetchRandomDog() async -> Result<DogResponse, Error> {
    let request = RandomDogRequest()
    let result = await fetchJSON(request)
    return result
}

public func fetchRandomDogWithSleep(label: String = "") async -> Result<DogResponse, Error> {
    let request = RandomDogRequest()
    if !label.isEmpty {
        print("fetchRandomDogWithSleep \(label)")
    }
    let result = await fetchJSON(request)
    try? await Task.sleep(nanoseconds: 2_000_000_000)
    if !label.isEmpty {
        print("fetchRandomDogWithSleep \(label) DONE")
    }
    return result
}

public func fetchDogImageData(_ dog: DogResponse) async -> Data? {
    let result = await fetchData(URLDataRequest(url: dog.message))
    return try? result.get()
}


