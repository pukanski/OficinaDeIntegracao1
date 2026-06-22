import { Injectable } from '@angular/core';
import { createClient, SupabaseClient } from '@supabase/supabase-js';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private supabase: SupabaseClient;

    constructor() {
        this.supabase = createClient(environment.supabaseUrl, environment.supabaseAnonKey);
    }

    async login(email: string, senha: string) {
        const { data, error } = await this.supabase.auth.signInWithPassword({
            email: email,
            password: senha,
        });

        if (error) {
            throw error;
        }

        if (data.session) {
            localStorage.setItem('jwt_token', data.session.access_token);
        }

        return data;
    }

    logout() {
        localStorage.removeItem('jwt_token');
        this.supabase.auth.signOut();
    }
}