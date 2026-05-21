# JWT Authentication & Authorization Guide

## Overview
Ce backend utilise JWT (JSON Web Tokens) pour l'authentification et l'autorisation basée sur les rôles.

### Utilisateurs et Rôles

#### Admin
- Accès complet à toutes les ressources
- Peut gérer les Societes, Categories, ClesDeRepartition, etc.
- Peut gérer tous les UserSociete
- Peut supprimer toutes les ressources

#### Societe
- Accès limité à la visualisation et modification
- Peut lire et mettre à jour LigneFinanciere (montants, quantités)
- Peut gérer Produit, FamilleProduit, TypeClient
- Peut gérer les UserSociete de sa propre Societe
- Ne peut pas supprimer les ressources
- Ne peut pas voir les données des autres Societes

---

## Endpoints

### Authentication

#### Admin Login
```http
POST /api/admin/login
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "password123"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": "2025-01-15T20:00:00Z",
  "id": 1,
  "userType": "Admin"
}
```

#### Societe Login
```http
POST /api/societe/login
Content-Type: application/json

{
  "email": "societe@example.com",
  "password": "password123"
}

Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": "2025-01-15T20:00:00Z",
  "id": 5,
  "userType": "Societe"
}
```

---

### Using the Token

Ajoute le token à chaque requête dans le header `Authorization`:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## Frontend Implementation (Angular)

### 1. Créer un AuthService
```typescript
// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) {}

  loginAdmin(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/admin/login`, { email, password });
  }

  loginSociete(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/societe/login`, { email, password });
  }

  saveToken(response: any): void {
    localStorage.setItem('token', response.token);
    localStorage.setItem('userType', response.userType);
    localStorage.setItem('userId', response.id);
    localStorage.setItem('expiresIn', response.expiresIn);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserType(): string | null {
    return localStorage.getItem('userType');
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userType');
    localStorage.removeItem('userId');
    localStorage.removeItem('expiresIn');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}
```

### 2. Créer un HTTP Interceptor
```typescript
// jwt.interceptor.ts
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();
    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
    return next.handle(req);
  }
}
```

### 3. Ajouter le Interceptor dans app.module.ts
```typescript
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from './jwt.interceptor';

@NgModule({
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ]
})
export class AppModule {}
```

### 4. Confirmation Dialog pour Modifications
```typescript
// confirm-dialog.component.ts
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-dialog',
  template: `
    <h2 mat-dialog-title>{{ data.title }}</h2>
    <mat-dialog-content>{{ data.message }}</mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">Annuler</button>
      <button mat-raised-button color="primary" (click)="onConfirm()">{{ data.confirmText }}</button>
    </mat-dialog-actions>
  `
})
export class ConfirmDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
}
```

### 5. Exemple: Modifier Montant LigneFinanciere
```typescript
// ligne-financiere.component.ts
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from './confirm-dialog.component';

@Component({
  selector: 'app-ligne-financiere',
  templateUrl: './ligne-financiere.component.html'
})
export class LigneFinanciereComponent {
  constructor(
    private http: HttpClient,
    private dialog: MatDialog
  ) {}

  updateMontant(id: number, newMontant: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Modification du Montant',
        message: `Êtes-vous sûr de vouloir modifier le montant à ${newMontant}?`,
        confirmText: 'Modifier'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.http.patch(`/api/lignefinanciere/${id}/montant`, newMontant)
          .subscribe(
            () => alert('Montant mis à jour'),
            error => alert('Erreur: ' + error)
          );
      }
    });
  }
}
```

---

## Permissions Summary

| Ressource | Admin | Societe |
|-----------|-------|---------|
| Admin CRUD | ? | ? |
| Societe View All | ? | ? |
| Societe Own Account | ? | ? |
| LigneFinanciere CRUD | ? | ? (no delete) |
| Produit CRUD | ? | ? (no delete) |
| FamilleProduit CRUD | ? | ? (no delete) |
| TypeClient CRUD | ? | ? (no delete) |
| UserSociete Own | ? | ? (no delete) |
| UserSociete All | ? | ? |

---

## Token Expiration
- Tokens expirent après **8 heures**
- À la réception d'une erreur 401, l'utilisateur doit se reconnecter

## Configuration
Voir `appsettings.json` pour:
- `SecretKey`: Clé de signature JWT (à garder secrète en production)
- `Issuer`: Validateur d'émetteur du token
- `Audience`: Destinataire valide du token
