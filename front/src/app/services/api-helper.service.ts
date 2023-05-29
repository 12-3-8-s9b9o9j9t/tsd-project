import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable, lastValueFrom } from 'rxjs';
import { HttpClient, HttpResponse } from '@angular/common/http';

const base_url: string = environment.api_url;

@Injectable({
  providedIn: 'root',
})
export class ApiHelperService {
  constructor(private http: HttpClient) {}

  public get({
    endpoint,
    queryParams = {},
    responseType = 'json', // Valeur par défaut : JSON
  }: {
    endpoint: string;
    queryParams?: any;
    responseType?: 'json' | 'text' | 'blob' | 'arraybuffer';
  }): Promise<any> {
    return this.request({ endpoint, method: 'GET', queryParams, responseType });
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
    responseType = 'json',
  }: {
    endpoint: string;
    method?: string;
    data?: object;
    queryParams?: any;
    responseType?: 'json' | 'text' | 'blob' | 'arraybuffer';
  }): Promise<any> {
    const methodWanted = method.toLowerCase();

    const url = base_url + endpoint;

    const requestOptions = {
      params: queryParams,
      responseType: responseType as 'json', // Conversion de string à type responseType
    };

    console.log(method, url, JSON.stringify(requestOptions), JSON.stringify(data));

    let req: Observable<HttpResponse<any>>;
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

