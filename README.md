# XXLStores API 🚀

Bienvenue sur **XXLStores API** – une API ultra-performante pour la gestion de magasin, avec une architecture **CQRS** robuste et des fonctionnalités avancées comme l'authentification **Role-Based Access Control (RBAC)**, la traçabilité complète des actions et l'intégration de **Swagger** pour une expérience de développement fluide et intuitive ! 🛠️

---

## 🎯 **Vue d'ensemble**
L'API XXLStores est conçue pour gérer efficacement les produits, les stocks et les commandes avec une séparation claire entre les **Commandes** (modifications de données) et les **Requêtes** (lecture des données). Cette architecture garantit des performances optimales tout en assurant la flexibilité nécessaire pour évoluer.

### Points forts :
- **CQRS** : Gestion optimisée des lectures et écritures grâce à la séparation des responsabilités.
- **Authentification RBAC avancée** : Contrôlez l'accès à vos ressources avec des rôles personnalisés.
- **Pagination** : Naviguez facilement dans de grands volumes de données.
- **Traçabilité** : Suivi des actions pour plus de transparence et de sécurité.
- **Swagger UI** : Découvrez et testez facilement l'API via une interface interactive.

---

## 🛠 **Caractéristiques avancées**
### 🔐 **Authentification basée sur les rôles (RBAC)**
La sécurité est au cœur de l'API XXLStores ! L’API repose sur une authentification **JWT** combinée avec un système de **Role-Based Access Control (RBAC)** puissant. Cela signifie que **chaque utilisateur** se voit attribuer un rôle spécifique (par exemple, **Admin**, **Manager**, **Client**), ce qui détermine les actions qu’il peut effectuer au sein du système.

#### 🎩 **Super-pouvoirs des Admins**
Les administrateurs (**Admin**) possèdent un contrôle total sur le système :
- **Définir des rôles personnalisés** : Créez des rôles selon vos besoins (lecture seule, modification, etc.).
- **Gérer l’accès aux ressources** : Choisissez qui peut accéder à quelles ressources et avec quelles permissions (lecture, écriture ou les deux).
- **Déléguer des permissions** : Attribuez ou retirez des rôles à différents utilisateurs, en assurant une gestion fine des accès à l'API.

#### Exemples de flux d'authentification :
- **Accès lecture seule** pour les utilisateurs réguliers : un utilisateur sans privilège d’écriture peut seulement lire les données.
- **Contrôle total** pour les administrateurs : Ils peuvent ajouter, modifier, ou supprimer des produits, gérer les utilisateurs et leurs rôles.

### 📑 **Pagination intégrée**
Grâce à la **pagination**, l’API gère efficacement les réponses volumineuses, vous permettant de récupérer les données en lots, améliorant ainsi les performances et l'expérience utilisateur :
```json
{
  "page": 1,
  "pageSize": 10,
  "totalItems": 200,
  "items": [...]
}
```

### 📈 **Traçabilité et audit**
L'API garde une trace de chaque action effectuée par les utilisateurs :
- **Enregistrement des modifications** : Qui a modifié quoi, et quand ?
- **Audit des accès** : Suivi des requêtes d’accès, garantissant que chaque utilisateur n’accède qu’aux ressources autorisées.
- Cela permet un **monitoring** poussé et offre une vue d'ensemble des interactions avec le système.

---

## 🚀 **Démarrage rapide**

### Prérequis
- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Microsoft SQL Server](https://www.microsoft.com/sql-server)
- [Visual Studio](https://visualstudio.microsoft.com/) ou tout autre IDE compatible avec C#

### Installation
1. Clonez le dépôt dans votre environnement local :
   ```bash
   git clone https://github.com/Stephane-AmStrong/XXLStores_API.git
   cd XXLStores_API
   ```

2. Configurez votre base de données dans `appsettings.json` :
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=XXLStoresDB;Trusted_Connection=True;"
   }
   ```

3. Appliquez les migrations à la base de données :
   ```bash
   dotnet ef database update
   ```

4. Lancez l’API :
   ```bash
   dotnet run
   ```

---

## 🔍 **Swagger UI**
**Swagger** est intégré dans l'API pour une expérience de développement interactive ! 🚀

Avec **Swagger**, vous pouvez **explorer** l'intégralité des endpoints de l'API via une interface utilisateur propre et intuitive. Swagger vous permet également de **tester les requêtes en direct** et de vérifier les réponses, tout cela sans quitter votre navigateur.

- **URL Swagger** : Une fois l'API démarrée, accédez à Swagger via : `http://localhost:5000/swagger`
- Vous pourrez y voir tous les **endpoints** disponibles, les types de requêtes, et tester directement les fonctionnalités (comme l’authentification, la gestion des produits, etc.).

![Swagger Screenshot](https://swagger.io/assets/images/swagger-logo.svg)

---

## 📚 **Endpoints API**
| Méthode | Route                   | Description                              | Authentification |
|---------|-------------------------|------------------------------------------|------------------|
| POST    | `/api/auth/login`        | Authentification de l'utilisateur        | ❌               |
| GET     | `/api/products`          | Récupérer la liste des produits (avec pagination) | ✅               |
| POST    | `/api/products`          | Créer un nouveau produit (rôle admin)    | ✅               |
| PUT     | `/api/products/{id}`     | Mettre à jour un produit existant        | ✅               |
| DELETE  | `/api/products/{id}`     | Supprimer un produit (rôle admin)        | ✅               |

❌ non authentication needed
✅ authentication needed
---

<!-- ## 🔧 **Tests et débogage**
1. Utilisez **Postman** ou **Swagger** pour tester les différentes routes.
2. Exécutez les tests unitaires :
   ```bash
   dotnet test
   ```

--- -->

## 🧑‍🔧 **Contribuer**
Les contributions sont toujours les bienvenues ! N’hésitez pas à soumettre des issues ou à proposer des pull requests pour améliorer l'API. Assurez-vous de suivre les bonnes pratiques et les conventions de codage.

---

## 📜 **Licence**
Ce projet est sous licence [MIT](LICENSE), ce qui signifie que vous êtes libre de l'utiliser, de le modifier et de le partager.

---

## 🎉 **Rejoignez la communauté XXLStores !**
XXLStores API est bien plus qu’une simple API – c’est un **écosystème puissant et flexible**, conçu pour grandir avec vos besoins. Qu'il s'agisse de gérer une boutique en ligne ou un réseau de magasins physiques, XXLStores API met à votre disposition tout ce dont vous avez besoin, avec la sécurité, la performance, et la facilité d'utilisation au cœur de l'expérience ! 🔥
