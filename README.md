<img src="wwwroot/img/jsi-logo.png" alt="" width="80" />

# Just Short It (damnit)!

The most KISS single user URL shortener there is.

![](https://img.shields.io/github/license/miawinter98/just-short-it?color=green&style=for-the-badge)
![](https://img.shields.io/github/forks/miawinter98/just-short-it?label=github%20forks&logo=github&style=for-the-badge)
![](https://img.shields.io/github/stars/miawinter98/just-short-it?label=github%20stars&color=yellow&logo=github&style=for-the-badge)

![](https://img.shields.io/docker/pulls/miawinter/just-short-it?color=informational&logo=docker&style=for-the-badge)
![](https://img.shields.io/docker/v/miawinter/just-short-it/latest?logo=docker&label=latest&style=for-the-badge)
![](https://img.shields.io/docker/v/miawinter/just-short-it/latest-alpine?logo=docker&label=latest%20alpine&style=for-the-badge)

## To simply run Just Short It in a container run:
```shell
docker run -e JSI_BaseUrl=<your-url> \
           -e JSI_Account__Username=<your-username> \
           -e JSI_Account__Password=<your-password> \
           -p 80:8080
           miawinter/just-short-it:latest
```


## In Docker Compose:
```yaml
version: '3.4'

services:
  just-short-it:
    container_name: JustShortIt
    image: miawinter/just-short-it:latest
    ports:
      - "80:8080"
    environment:
      - "JSI_BaseUrl=<your-url>"
      - "JSI_Account__Username=<your-username>"
      - "JSI_Account__Password=<your-password>"
```

## Redis

By default Just Short It saves all the redirects in a in-memory distributed Cache, which get's lost 
whenever the container restarts, so if you want to keep your redirects you wanna use redis.

You can configure the connection to redis using the environment variables `JSI_Redis__ConnectionString` 
and optional `JSI_Redis__InstanceName` (default is "JustShortIt").

If you want to run both with compose, the most simple setup looks like this:

```yaml
version: '3.4'

services:
  just-short-it:
    container_name: JustShortIt
    image: miawinter/just-short-it:latest
    ports:
      - "80:8080"
    environment:
      - "JSI_BaseUrl=<your-url>"
      - "JSI_Account__Username=<your-username>"
      - "JSI_Account__Password=<your-password>"
      - "JSI_Redis__ConnectionString=redis,password=<your-redis-password>"
    depends_on:
      - redis
  redis:
    container_name: Redis
    image: redis:alpine
    environment:
      - "REDIS_PASSWORD=<your-redis-password>"
    volumes:
      - redis:/data

volumes:
  redis:
```

There you go, now your urls survive a restart!

# Https

Just Short It! is not supporting Https, I reconmend using a reverse Proxy for hosting
that handles SSL. I can highly reconmend jwilders 
[nginx-proxy](https://github.com/nginx-proxy/nginx-proxy) togehter with 
[acme-companion](https://github.com/nginx-proxy/acme-companion), 
there is no easier way to get a reverse proxy with automatic certificate renewal.

Here is an Example of how to use Just Short It! togehter with nginx-proxy:

```yaml
version: '3.4'

services:
  # Just Short It
  just-short-it:
    container_name: JustShortIt
    image: miawinter/just-short-it:latest
    environment:
      - "JSI_BaseUrl=<your-url>"
      - "JSI_Account__Username=<your-username>"
      - "JSI_Account__Password=<your-password>"
      - "JSI_Redis__ConnectionString=redis,password=<your-redis-password>"
    environment:
      - "VIRTUAL_HOST=<your-url>"
      - "VIRTUAL_PORT=8080"
      - "LETSENCRYPT_HOST=<your-url>"
    depends_on:
      - redis
      - acme-companion
  redis:
    container_name: Redis
    image: redis:alpine
    environment:
      - "REDIS_PASSWORD=<your-redis-password>"
    volumes:
      - redis:/data

  # nginx-proxy with acme-companion
  nginx-proxy:
    container_name: nginx-proxy
    restart: unless-stopped
    image: jwilder/nginx-proxy:alpine
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - /var/run/docker.sock:/tmp/docker.sock:ro
      - certs:/etc/nginx/certs:ro
      - conf:/etc/nginx/conf.d
      - vhost:/etc/nginx/vhost.d
      - html:/usr/share/nginx/html
    environment:
      - "DHPARAM_GENERATION=false" # Not sure you need this actually
      - "DISABLE_ACCESS_LOGS" # Always nice to comply with GDPR
  acme-companion:
    container_name: acme-companion
    restart: unless-stopped
    image: nginxproxy/acme-companion
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock:ro
      - certs:/etc/nginx/certs:rw
      - conf:/etc/nginx/conf.d
      - vhost:/etc/nginx/vhost.d
      - html:/usr/share/nginx/html
      - /etc/acme.sh:/etc/acme.sh
    environment:
      - "DEFAULT_EMAIL=<your-email>"
      - "NGINX_PROXY_CONTAINER=nginx-proxy"
    depends_on:
      - nginx

volumes:
  # Just Short It!
  redis:
  # Proxy
  certs:
  conf:
  vhost:
  html:

```

# License and Attribution

Just Short It by [Mia Winter](https://miawinter.de/) is licensed under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).  

Just Short It uses [tailwindcss](https://tailwindcss.com/), licensed under the [MIT License](https://github.com/tailwindlabs/tailwindcss/blob/master/LICENSE)  
Just Short It uses [daisyUI](https://daisyui.com/), licensed under the [MIT License](https://github.com/saadeghi/daisyui/blob/master/LICENSE)  
Just Short It uses [PostCSS](https://postcss.org/), licensed under the [MIT License](https://github.com/postcss/postcss/blob/main/LICENSE)  
Just Short It uses [PostCSS CLI](https://github.com/postcss/postcss-cli/), licensed under the [MIT License](https://github.com/postcss/postcss-cli/blob/master/LICENSE)  
Just Short It uses [autoprefixer](https://github.com/postcss/autoprefixer/), licensed under the [MIT License](https://github.com/postcss/autoprefixer/blob/main/LICENSE)  
Just Short It uses [cssnano](https://cssnano.co/), licensed under the [MIT License](https://github.com/cssnano/cssnano/blob/master/LICENSE-MIT)  
Just Short It uses [heroicons](https://heroicons.com/), licensed under the [MIT License](https://github.com/tailwindlabs/heroicons/blob/master/LICENSE)

Copyright (c) 2024 Mia Winter
