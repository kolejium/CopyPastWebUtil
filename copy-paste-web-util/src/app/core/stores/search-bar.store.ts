import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";


@Injectable({
    providedIn: 'any',
})
export class SearchBarStore {
    private _query: BehaviorSubject<string> = new BehaviorSubject('');

    get query() {
        return this._query.asObservable();
    }

    setQuery(value: string) : void {
        this._query.next(value);
    }
}