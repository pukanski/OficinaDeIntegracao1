import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    // 1. Tenta restaurar a sessão do localStorage
    const estaLogado = authService.restoreSession();

    if (!estaLogado) {
        router.navigate(['/login']);
        return false;
    }

    const expectedRole = route.data?.['role'];
    const userRole = authService.role;

    if (expectedRole && userRole !== expectedRole) {
        console.warn(`Tentativa de acesso bloqueada. Rota exige: ${expectedRole}. Perfil atual: ${userRole}.`);
        switch (userRole) {
            case 'admin':
                router.navigate(['/admin/painel']);
                break;
            case 'professor':
                router.navigate(['/professor/dashboard']);
                break;
            case 'aluno':
                router.navigate(['/aluno/dashboard']);
                break;
            default:
                authService.logout();
                router.navigate(['/login']);
                break;
        }
        return false;
    }

    return true;
};