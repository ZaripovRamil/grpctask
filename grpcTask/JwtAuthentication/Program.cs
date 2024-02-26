using Grpc.Core;
using Grpc.Net.Client;
using Jwt;
using Secret;
using Request = Jwt.Request;

using var channel = GrpcChannel.ForAddress("http://localhost:9999");

var jwtServiceClient = new JwtService.JwtServiceClient(channel);

var serverData = jwtServiceClient.GetJwt(new Request{Username = "admin"});

if (serverData.Token is not {} token) throw new NullReferenceException();

var authMetadata = new Metadata { { "Authorization", $"Bearer {token}" } };

var secretServiceClient = new SecretService.SecretServiceClient(channel);

var secret = secretServiceClient.GetSecret(new Secret.Request(), headers: authMetadata);

Console.WriteLine($"Secret data: {secret.Data}");