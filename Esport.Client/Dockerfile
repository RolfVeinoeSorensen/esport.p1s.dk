# Esport.Client/Dockerfile
# Stage 1: Build the Angular application
# FROM arm64v8/node:alpine AS builder - MAC M CPUs
# FROM node:alpine AS builder - AMD64
FROM --platform=${BUILDPLATFORM} alpine AS builder
WORKDIR /app

# Make sure we got brotli
RUN apk update
RUN apk add --upgrade brotli
RUN apk add --update nodejs npm

# NPM install and build
COPY package.json package-lock.json ./
RUN npm install -g @angular/cli
RUN npm install
COPY . ./
#RUN npm run build
CMD ["ng", "build", "--configuration", "production"]
RUN cd /app/dist && find . -type f -exec brotli {} \;

# Stage 2: Serve the application with Nginx
FROM --platform=${BUILDPLATFORM} alpine

RUN apk add --no-cache brotli nginx nginx-mod-http-brotli

# Minimal config
COPY nginx/default.conf /etc/nginx/http.d/default.conf

# Actual data
COPY --from=builder /app/dist/Esport.client/browser /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]
EXPOSE 80

# Test compression: curl -H 'Accept-Encoding: br' -I http://localhost:4301
