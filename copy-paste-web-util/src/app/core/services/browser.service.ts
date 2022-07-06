import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { ApiService } from "./api.service";


@Injectable({
    providedIn: 'any',
})
export class BrowserService {
    constructor(private api: ApiService) { }

    replaceProxy(document: string) {

    }

    isRelativeLink(link: string) : boolean {
        return link.startsWith('/');
    }
}