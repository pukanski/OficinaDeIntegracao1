FROM node:22

WORKDIR /app

ARG SUPABASE_URL
ARG SUPABASE_ANON_KEY
ARG GATEWAY_URL

ENV SUPABASE_URL=$SUPABASE_URL
ENV SUPABASE_ANON_KEY=$SUPABASE_ANON_KEY
ENV GATEWAY_URL=$GATEWAY_URL

COPY package*.json ./

RUN npm install

RUN apt-get update && apt-get install -y gettext-base

COPY . .

RUN envsubst < src/environments/environment.template.ts > src/environments/environment.ts

EXPOSE 4200

CMD ["npm", "start", "--", "--host", "0.0.0.0"]