#!/bin/sh

# Install packages
npm install

# Create a private key
openssl genpkey -algorithm EC -pkeyopt ec_paramgen_curve:P-256 > private_key.pem

# Extract the public key fromt the private key
openssl pkey -pubout -in private_key.pem > public_key.pem

# Generate a self-signed certificate for the key pair
openssl req -x509 -key private_key.pem -subj /CN=client.example.com -days 1000 > certificate.pem

# Convert fhe format of the public key from PEM to JWK
node_modules/eckles/bin/eckles.js public_key.pem > public_key.jwk

# Generate a JWK including the certificate
CERT=$(sed /-/d certificate.pem | tr -d \\n)

jq ".+{\"x5c\":[\"$CERT\"]}" public_key.jwk > pub+cert.jwk