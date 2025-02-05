# TP1_BDAvancee

## Description
TP1_BDAvancee est une application Windows Forms qui permet de gérer des articles. L'application utilise ADO.NET pour interagir avec une base de données SQL Server. Elle suit une architecture MVC (Model-View-Controller) pour séparer les préoccupations et améliorer la maintenabilité du code.

## Fonctionnalités
- Ajouter un nouvel article
- Mettre à jour un article existant
- Supprimer un article
- Afficher tous les articles valides
- Rechercher des articles par nom
- Naviguer entre les articles

## Prérequis
- .NET Framework 4.7.2
- SQL Server

## Installation
1. Clonez le dépôt :
   
   git clone https://github.com/raphaelsrcunha/TP1-BDAvancee.git

2. Ouvrez le projet dans Visual Studio.
3. Assurez-vous que la chaîne de connexion à la base de données est correcte dans le fichier `DatabaseConnection.cs`.

## Structure du Projet
Le projet est structuré comme suit :

### Fichiers Principaux
- `Form1.cs` : Le formulaire principal de l'application.
- `FormUpdateArticle.cs` : Le formulaire pour mettre à jour un article.
- `ArticleManager.cs` : Le contrôleur qui gère les opérations CRUD pour les articles.
- `DatabaseConnection.cs` : Gère la connexion à la base de données.
- `Article.cs` : Le modèle représentant un article.

### Détails des Fichiers

#### Form1.cs
Le fichier `Form1.cs` contient le code pour le formulaire principal de l'application. Il gère l'affichage des articles, la navigation entre les articles, et les interactions avec l'utilisateur pour ajouter, mettre à jour et supprimer des articles.

#### FormUpdateArticle.cs
Le fichier `FormUpdateArticle.cs` contient le code pour le formulaire de mise à jour des articles. Il permet de charger les détails d'un article, de mettre à jour ses informations et de gérer l'affichage de l'image associée à l'article.

#### ArticleManager.cs
Le fichier `ArticleManager.cs` contient la logique de gestion des articles. Il inclut des méthodes pour ajouter, supprimer, mettre à jour et récupérer des articles depuis la base de données.

#### DatabaseConnection.cs
Le fichier `DatabaseConnection.cs` gère la connexion à la base de données SQL Server. Il fournit une méthode pour établir la connexion et peut être utilisé par les autres classes pour interagir avec la base de données.

#### Article.cs
Le fichier `Article.cs` définit le modèle représentant un article. Il inclut des propriétés pour les différents attributs d'un article, comme l'ID, le code, le nom, la description, la marque, la catégorie, le prix, l'URL de l'image et la validité.

## Utilisation
1. Lancez l'application depuis Visual Studio.
2. Utilisez les boutons pour ajouter, mettre à jour ou supprimer des articles.
3. Naviguez entre les articles en utilisant les boutons de navigation.
4. Recherchez des articles en utilisant la barre de recherche.

## Contribuer
Les contributions sont les bienvenues ! Veuillez suivre les étapes suivantes pour contribuer :
1. Forkez le dépôt.
2. Créez une branche pour votre fonctionnalité (`git checkout -b feature/ma-fonctionnalite`).
3. Commitez vos modifications (`git commit -m 'Ajout de ma fonctionnalité'`).
4. Poussez votre branche (`git push origin feature/ma-fonctionnalite`).
5. Ouvrez une Pull Request.