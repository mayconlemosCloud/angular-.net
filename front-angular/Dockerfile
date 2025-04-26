# Use Node.js 18 
FROM node:18.19.0

# Install Angular CLI globally
RUN npm install -g @angular/cli

# Set up the working directory
WORKDIR /
RUN mkdir front-angular
WORKDIR /front-angular

# Define environment variables for app creation
ENV APP_NAME 'front-angular'
ENV ROUTING 'true'
ENV STANDALONE 'false'
ENV STRICT 'true'
ENV STYLE 'scss'

# Command to create and serve the Angular app
CMD ng new $APP_NAME --routing=$ROUTING --standalone=$STANDALONE --strict=$STRICT --style=$STYLE \
    && mv $APP_NAME/* . \
    && rm -rf $APP_NAME \
    && ng serve --host 0.0.0.0 --port 4200

# Expose the Angular development server port
EXPOSE 4200
