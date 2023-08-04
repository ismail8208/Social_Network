import { Injectable } from '@angular/core';
// import { jwt_decode } from 'jsonwebtoken';
import jwt_decode from 'jwt-decode';

// import * as jwt_decode from "jwt-decode";

@Injectable({
    providedIn: 'root'
})
export class TokenService {
    getDecodedAccessToken(token: string): any {
        try {
            return jwt_decode(token);
        } catch(Error) {
            return null;
        }
    }
}
