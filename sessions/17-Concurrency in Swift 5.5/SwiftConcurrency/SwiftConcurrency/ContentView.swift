//
//  ContentView.swift
//  SwiftConcurrency
//
//  Created by Michal Melich on 22/09/2021.
//

import SwiftUI

class ViewModel: ObservableObject {
    @Published var text: String = "" {
        didSet {
            print("test didSet \(Thread.isMainThread)")
        }
    }
    
    func oldWayUpdate(text: String) {
        self.text = text
    }
    
    func updateText(text: String) async -> Bool {
        self.text = text
        return true
    }
    
    @MainActor func updateTextWithActor(text: String) async -> Bool {
        self.text = text
        return true
    }
}

struct ContentView: View {
    @ObservedObject var viewModel = ViewModel()
    var body: some View {
        NavigationView {
            List {
                Button("Execute tasks in parallel") {
                    Tasks.performParallelTasks()
                }.padding()
                
                Button("Temperature Logger Class") {
                    Tasks.performClassTask()
                }.padding()
                
                Button("Temperature Logger Actor") {
                    Tasks.performActorTask()
                }.padding()
            
                Button("DispatchQueue Update") {
                    DispatchQueue.global().async {
                        // do some work
                        // and update ui
                        DispatchQueue.main.async {
                            viewModel.oldWayUpdate(text: "DispatchQueue Update")
                        }
                    }
                }.padding()
                
                Button("Async update") {
                    Task {
                        async let r1 = viewModel.updateText(text: "Async update")
                    }
                }.padding()
                
                Button("MainActor update") {
                    Task {
                        async let r1 = viewModel.updateTextWithActor(text: "Async update @MainActor")
                    }
                }.padding()
                
                Text("\(viewModel.text)").padding()
            }
        }.navigationTitle("Concurrency")
        
        
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView()
    }
}
