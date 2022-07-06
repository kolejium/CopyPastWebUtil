import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import apiJson from '../../../environments/env.config.json';
import { Observable } from "rxjs";


@Injectable({
    providedIn: 'any',
})
export class ApiService {
    constructor(private http: HttpClient) {

    }

    getUrlForGetSiteByUrl(url: string) : string {
        return `${apiJson.Api.ExtenalShare.BaseUrl}${apiJson.Api.ExtenalShare.Proxy}?url=${url}&proxy=${apiJson.Api.ExtenalShare.BaseUrl}${apiJson.Api.ExtenalShare.Proxy}?url=`;
    }

    getSiteByUrl(url: string) : Observable<any> {
        let params = new HttpParams().set("url", url).set("proxy", `${apiJson.Api.ExtenalShare.BaseUrl}${apiJson.Api.ExtenalShare.Proxy}?url=`);
        let query = `${apiJson.Api.ExtenalShare.BaseUrl}${apiJson.Api.ExtenalShare.Proxy}`;
        return this.http.get(query, { params: params, responseType: "text" });
    }
}