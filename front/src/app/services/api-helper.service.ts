import { Injectable } from '@angular/core';
import { environment } from './../environments/environment';
import { Observable, lastValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';

const dev_base_url: string = 'http://localhost:5225';
const prod_base_url: string = 'https://api.localhost:80';   // Modify when deploying to production

@Injectable({
  providedIn: 'root',
})
export class ApiHelperService {
  constructor(private http: HttpClient) {}

  public get({
    endpoint,
    queryParams = {},
  }: {
    endpoint: string;
    queryParams?: any;
  }): Promise<any> {environment
    return this.request({ endpoint, method: 'GET', queryParams });
  }

  public post({
    endpoint,
    data = {},
    queryParams = {},
  }: {
    endpoint: string;
    data?: any;
    queryParams?: any;
  }): Promise<any> {
    return this.request({ endpoint, method: 'POST', data, queryParams });
  }

  public put({
    endpoint,
    data = {},
    queryParams = {},
  }: {
    endpoint: string;
    data?: any;
    queryParams?: any;
  }): Promise<any> {
    return this.request({ endpoint, method: 'PUT', data, queryParams });
  }

  public delete({
    endpoint,
    data = {},
    queryParams = {},
  }: {
    endpoint: string;
    data?: any;
    queryParams?: any;
  }): Promise<any> {
    return this.request({ endpoint, method: 'DELETE', data, queryParams });
  }

  public async request({
    endpoint,
    method = 'GET',
    data = {},
    queryParams = {},
  }: {
    endpoint: string;
    method?: string;
    data?: object;
    queryParams?: any;
  }): Promise<any> {
    const methodWanted = method.toLowerCase();

    const url = dev_base_url + endpoint;

    const requestOptions = {
      params: queryParams,
    };

    console.log(method, url, JSON.stringify(requestOptions), JSON.stringify(data));

    let req: Observable<any>;
    if (methodWanted === 'get') {
      req = this.http.get(url, { ...requestOptions, observe: 'response' });
    } else if (methodWanted === 'post') {
      req = this.http.post(url, data, {
        ...requestOptions,
        observe: 'response',
      });
    } else if (methodWanted === 'put') {
      req = this.http.put(url, data, {
        ...requestOptions,
        observe: 'response',
      });
    } else {
      req = this.http.delete(url, { ...requestOptions, observe: 'response' });
    }

    if (!req) {
      throw new Error(`error calling ${url} with method ${methodWanted}`);
    }

    return await lastValueFrom(req).then((res) => {
      return res.body;
    });
  }
}
