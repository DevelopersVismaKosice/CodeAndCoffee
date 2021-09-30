import UIKit

public func fetchJSON<R: Request>(_ request: R, completion: @escaping (Result<R.Response, Error>) -> Void) {
    let url = URL(string: request.url)!
    print("Fetching url: \(url)")
    let task = URLSession.shared.dataTask(with: url) { data, response, error in
        let decoder = JSONDecoder()
        if let data = data {
            do {
                let result = try decoder.decode(R.Response.self, from: data)
                completion(.success(result))
            } catch {
                completion(.failure(error))
            }
            
        }
    }
    task.resume()
}

public func fetchData<R: Request>(_ request: R, completion: @escaping (Result<R.Response, Error>) -> Void) where R.Response == Data {
    let url = URL(string: request.url)!
    print("Fetching url: \(url)")
    let task = URLSession.shared.dataTask(with: url) { data, response, error in
        
        if let error = error {
            completion(.failure(error))
        } else if let data = data {
            completion(.success(data))
        } else {
            completion(.failure(ResponseError.NoData))
        }
    }
    task.resume()
}

public func fetchRandomDog(completion: @escaping (Result<DogResponse, Error>) -> Void) {
    let request = RandomDogRequest()
    
    fetchJSON(request) { result in
        completion(result)
    }
    
}

public func fetchDogImageData(_ dog: DogResponse, completion: @escaping (Data?) -> Void) {
    let request = URLDataRequest(url: dog.message)
    
    fetchData(request) { result in
        if case .success(let data) = result {
            completion(data)
        } else {
            print("fetchDogImage Error")
            completion(nil)
        }
    }
}
