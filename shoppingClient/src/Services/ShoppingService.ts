import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { ShoppingModel } from 'src/Models/ShoppingModel';

@Injectable({ providedIn: 'root' })
export class ShoppingService {
    constructor(private http: Http) { }

    public PushProduct(product: ShoppingModel): Promise<any> {
        let headrs = new Headers({ "Content-Type": "application/json" });
        let optinos = new RequestOptions({ headers: headrs });
        let body = JSON.stringify(product);
        let baseUrl: string = "http://localhost:1923/api/shopping";

        return this.http.post(baseUrl, body, optinos).toPromise()
    }
}