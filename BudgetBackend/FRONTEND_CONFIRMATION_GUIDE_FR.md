# ?? FRONTEND - PROMPTS/DIALOGUES DE CONFIRMATION

## ? Qu'est-ce qu'il faut faire?

Ajouter des **dialogues de confirmation** avant chaque action importante côté Angular (Frontend).

---

## ?? Liste des Actions Nécessitant une Confirmation

### 1?? MODIFICATION (All Users - Societe & Admin)
```
Action: Modifier un élément
Texte du prompt: "Êtes-vous sûr de vouloir modifier [nom_element]?"
Bouton: "Modifier" (bleu)
Exemple:
  - Modifier montant LigneFinancière
  - Modifier Produit
  - Modifier FamilleProduit
  - Modifier TypeClient
```

### 2?? SUPPRESSION (Admin Only)
```
Action: Supprimer un élément
Texte du prompt: "Êtes-vous SÛR de vouloir supprimer [nom_element]?
                  Cette action est IRRÉVERSIBLE!"
Bouton: "Supprimer" (ROUGE/DANGER)
Exemple:
  - Supprimer Produit
  - Supprimer FamilleProduit
  - Supprimer TypeClient
  - Supprimer LigneFinancière
```

### 3?? CRÉATION (Societe & Admin)
```
Action: Ajouter un nouvel élément
Texte du prompt: "Êtes-vous sûr de vouloir créer ce [type_element]?"
Bouton: "Créer" (vert)
Exemple:
  - Créer nouveau Produit
  - Créer nouvelle FamilleProduit
  - Créer nouveau TypeClient
```

### 4?? CHANGEMENT DE MONTANT (Societe & Admin)
```
Action: Modifier le montant
Texte du prompt: "Changer le montant de [ancien_montant]€ à [nouveau_montant]€?"
Afficher: "Différence: +[XXX]€" ou "-[XXX]€"
Bouton: "Modifier" (bleu)
```

---

## ?? Types de Dialogues à Créer

### Type 1: Simple Confirmation
```typescript
// Description: Dialog simple avec Oui/Non
// Utilisation: Modifications simples
// Couleur bouton: Bleu

Dialog {
  Titre: "Modification"
  Message: "Êtes-vous sûr?"
  [Annuler]  [Modifier] ? bleu
}
```

### Type 2: Danger Confirmation
```typescript
// Description: Dialog danger pour suppressions
// Utilisation: Supprimer des éléments
// Couleur bouton: ROUGE

Dialog {
  Titre: "Suppression"
  Message: "Êtes-vous SÛR? Cette action est irréversible!"
  [Annuler]  [Supprimer] ? ROUGE
}
```

### Type 3: Modification Montant
```typescript
// Description: Dialog spéciale pour montants
// Utilisation: Modification montants LigneFinancière
// Afficher: Différence de prix

Dialog {
  Titre: "Modifier le Montant"
  Message: "Montant actuel: 1000€"
  Input: [Nouveau montant: 1200€]
  Info: "Différence: +200€"
  [Annuler]  [Modifier] ? bleu
}
```

---

## ?? Où Ajouter les Confirmations

### Dans la Liste (Table/Grid)
```
???????????????????????????????????????
?  Produit     ? Prix   ? Actions    ?
???????????????????????????????????????
?  Produit 1   ? 100€   ? [Modifier] ? ? Click ? Dialog
?              ?        ? [Supprimer]? ? Click ? Dialog ROUGE
???????????????????????????????????????
```

### Dans le Formulaire
```
Formulaire d'édition:
[Nom: _____________]
[Prix: ____________]

[Enregistrer] ? Click ? Dialog "Modifier?"
[Annuler]
[Supprimer] ? Click ? Dialog ROUGE "Supprimer?"
```

### Dans les Actions Rapides
```
Bouton "Changer Montant" ? Click ? Dialog personnalisé montant
Bouton "Activer" ? Click ? Simple confirmation
Bouton "Désactiver" ? Click ? Simple confirmation
```

---

## ?? Flux d'Exécution Complet

```
1. Utilisateur clique sur "Modifier"
   ?
2. Dialog de confirmation s'ouvre
   ?? [Annuler] ? Rien ne se passe (ferme le dialog)
   ?? [Modifier] ? Continue...
   ?
3. Envoyer la requête HTTP au backend
   ?? Backend: Valide les permissions JWT
   ?? Backend: Exécute la modification
   ?? Backend: Retourne 200 OK ou erreur
   ?
4. Frontend reçoit la réponse
   ?? Si OK ? Afficher "? Modifié avec succès!"
   ?? Rafraîchir la liste
   ?? Rediriger vers liste
   
   ?? Si Erreur ? Afficher "? Erreur: [message]"
   ?? Rester sur le formulaire
```

---

## ?? Code Angular à Implémenter

### Service de Confirmation (déjà fourni)
```typescript
// confirmation.service.ts
this.confirmationService.confirm(
  "Titre du dialog",
  "Message à afficher"
).subscribe(confirmed => {
  if (confirmed) {
    // Utilisateur a cliqué "Confirmer"
    // Faire l'action
  }
  // Sinon: dialog fermé, rien ne se passe
});
```

### Exemple: Modifier un Produit
```typescript
onSaveProduct(): void {
  // 1. Demander confirmation
  this.confirmationService.confirmUpdate('ce produit').subscribe(confirmed => {
    
    // 2. Si utilisateur confirme
    if (confirmed) {
      this.http.put(`/api/produit/${this.product.id}`, this.product)
        .subscribe(
          () => {
            // 3. Succès
            alert('? Produit modifié!');
            this.router.navigate(['/produits']);
          },
          error => {
            // 3. Erreur
            alert('? Erreur: ' + error.error.message);
          }
        );
    }
    // Sinon: dialog fermé, rien ne se passe
  });
}
```

### Exemple: Supprimer un Produit
```typescript
onDeleteProduct(): void {
  // 1. Demander confirmation DANGER
  this.confirmationService.confirmDelete(
    'Supprimer le Produit',
    'Êtes-vous SÛR? Cette action est irréversible!'
  ).subscribe(confirmed => {
    
    // 2. Si utilisateur confirme
    if (confirmed) {
      this.http.delete(`/api/produit/${this.product.id}`)
        .subscribe(
          () => {
            // 3. Succès
            alert('? Produit supprimé!');
            this.router.navigate(['/produits']);
          },
          error => {
            // 3. Erreur
            alert('? Erreur: ' + error.error.message);
          }
        );
    }
  });
}
```

---

## ? Checklist - Qu'est-ce à Faire?

### Pour CHAQUE page/component:

- [ ] **Création d'élément**
  - [ ] Ajouter dialog "Êtes-vous sûr de créer?"
  - [ ] Montrer le bouton "Créer"

- [ ] **Modification d'élément**
  - [ ] Ajouter dialog "Êtes-vous sûr de modifier?"
  - [ ] Montrer détails (ancien vs nouveau)
  - [ ] Montrer le bouton "Modifier"

- [ ] **Suppression d'élément** (Admin only)
  - [ ] Ajouter dialog ROUGE "Êtes-vous SÛR?"
  - [ ] Afficher "Cette action est irréversible"
  - [ ] Montrer le bouton "Supprimer" (ROUGE)

- [ ] **Modification montant** (LigneFinancière)
  - [ ] Ajouter dialog "Montant actuel vs nouveau"
  - [ ] Afficher la différence (+/-)
  - [ ] Montrer le bouton "Modifier"

---

## ?? Pages à Mettre à Jour

### LigneFinancière
```
? Liste: Ajouter prompts sur [Modifier] et [Supprimer]
? Edit: Ajouter prompt avant save
? PATCH Montant: Ajouter dialog montant
```

### Produit
```
? Liste: Ajouter prompts sur [Modifier] et [Supprimer]
? Edit: Ajouter prompt avant save
? Create: Ajouter prompt avant create
```

### FamilleProduit
```
? Liste: Ajouter prompts
? Edit: Ajouter prompt
? Create: Ajouter prompt
```

### TypeClient
```
? Liste: Ajouter prompts
? Edit: Ajouter prompt
? Create: Ajouter prompt
```

### UserSociete
```
? Liste: Ajouter prompts (seulement own company pour Societe)
? Edit: Ajouter prompt
? Create: Ajouter prompt
? Filtre: Montrer seulement users de ma société si Societe
```

---

## ?? Résumé Simple

### Avant Chaque Action:

| Action | Prompt à Afficher | Couleur Bouton |
|--------|------------------|----------------|
| **Créer** | "Créer ce [type]?" | ?? Vert |
| **Modifier** | "Modifier [nom]?" | ?? Bleu |
| **Supprimer** | "Supprimer [nom]? IRRÉVERSIBLE!" | ?? ROUGE |
| **Montant** | "Changer: [ancien] ? [nouveau]?" | ?? Bleu |

---

## ?? Comment Implémenter

1. ? **Service de confirmation** - JE L'AI DÉJÀ FOURNI
   - Voir: `CONFIRMATION_FRONTEND_EXAMPLES.ts`

2. ? **À faire par vous:**
   - Copier le service dans votre projet Angular
   - Ajouter les imports Material
   - Ajouter les confirmations avant chaque action (POST, PUT, DELETE, PATCH)

3. ? **Résultat final:**
   - Chaque modification demande une confirmation
   - Les suppressions sont protégées (bouton ROUGE)
   - Utilisateur doit cliquer "Confirmer" pour exécuter l'action

---

## ?? Fichiers Référence Fournis

| Fichier | Contient |
|---------|----------|
| `CONFIRMATION_FRONTEND_EXAMPLES.ts` | Code Angular complet et prêt à copier |
| `ANGULAR_INTEGRATION_EXAMPLE.ts` | AuthService, Interceptor, Dialog |
| `JWT_GUIDE.md` | Instructions complètes frontend |

---

## ?? Points Clés à Retenir

? **Confirmation simple** = Dialog bleu "Êtes-vous sûr?"
? **Suppression** = Dialog ROUGE "IRRÉVERSIBLE!"  
? **Montant** = Dialog avec calcul de différence
? **All actions** = Valider avant d'envoyer au backend
? **Backend** = Valide les permissions JWT (sécurité double)

---

## ?? Ordre d'Implémentation Recommandé

1. **Semaine 1:** Setup Angular + AuthService + Interceptor
2. **Semaine 2:** Ajouter dialogues de confirmation simples
3. **Semaine 3:** Tester tous les prompts avec Admin et Societe
4. **Semaine 4:** Refinements et ajustements UI

---

**C'est tout ce que vous devez faire côté frontend! ??**
