// See https://aka.ms/new-console-template for more information
using System.Text.Json;

Console.WriteLine("Hello, World!");

result = JsonSerializer.Deserialize<InternalFaceDetectionResponse>(output, options);