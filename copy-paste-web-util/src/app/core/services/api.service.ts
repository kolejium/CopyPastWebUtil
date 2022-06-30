import { Injectable } from "@angular/core";
import { HttpClient, HttpEvent } from '@angular/common/http';

@Injectable({
    providedIn: 'any',
})
export class ApiService {
    constructor(private http: HttpClient) {

    }

    async getSiteByUrl(url: string) {
        await this.http.get(``);
    }
}