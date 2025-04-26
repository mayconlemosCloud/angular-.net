# Use Node.js to build the Angular app
FROM node:18.19.1 AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build 

# Use Nginx to serve the Angular app
FROM nginx:alpine
COPY --from=build /app/dist/front-angular /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/nginx.conf

# Expor a porta padrão do Nginx
EXPOSE 80

# Comando para iniciar o Nginx
CMD ["nginx", "-g", "daemon off;"]
