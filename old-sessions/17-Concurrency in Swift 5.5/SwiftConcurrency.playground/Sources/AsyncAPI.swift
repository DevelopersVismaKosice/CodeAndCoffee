import UIKit

enum FetchError: Error {
    case network
    case data
}

public func fetchJSON<R: Request>(_ request: R) async throws -> R.Response {
    let url = URL(string: request.url)!
    do {
        let (data, _) = try await URLSession.shared.data(from: url)
        do {
            let decoder = JSONDecoder()
            let result = try decoder.decode(R.Response.self, from: data)
            return result
        } catch {
            throw FetchError.data
        }
    
    } catch {
        throw FetchError.network
    }
}

public func fetchData<R: Request>(_ request: R) async throws -> R.Response where R.Response == Data {
    let url = URL(string: request.url)!

    do {
        let (data, _) = try await URLSession.shared.data(from: url)
        return data
    } catch {
        throw FetchError.network
    }
}

public func fetchRandomDog() async throws -> DogResponse {
    let request = RandomDogRequest()
    let result = try await fetchJSON(request)
    return result
}

public func fetchRandomDogWithSleep(label: String = "") async throws -> DogResponse {
    let request = RandomDogRequest()
    if !label.isEmpty {
        print("fetchRandomDogWithSleep \(label) STARTED")
    }
    let result = try await fetchJSON(request)
    try await Task.sleep(nanoseconds: 2_000_000_000)
    if !label.isEmpty {
        print("fetchRandomDogWithSleep \(label) DONE")
    }
    return result
}

public func fetchDogImageData(_ dog: DogResponse) async throws -> Data {
    print("fetchDogImageData")
    let result = try await fetchData(URLDataRequest(url: dog.message))
    return result
}
