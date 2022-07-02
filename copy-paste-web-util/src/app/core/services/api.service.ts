import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';

import apiJson from '../../../environments/env.config.json';
import { Observable } from "rxjs";


@Injectable({
    providedIn: 'any',
})
export class ApiService {
    constructor(private http: HttpClient) {

    }

    getSiteByUrl(url: string) : Observable<any> {
        return this.http.get(`${apiJson.Api.ExtenalShare.BaseUrl}${apiJson.Api.ExtenalShare.GetSiteHtml}?url=${url}`, { responseType: "text" });
    }

    proxySiteByUrl(url: string) : string {
        return `${apiJson.Api.ExtenalShare.BaseUrl}${apiJson.Api.ExtenalShare.ProxySite}?url=${url}`;
    }
}