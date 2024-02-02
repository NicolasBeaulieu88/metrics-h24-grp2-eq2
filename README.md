# metrics-h24-grp2-eq2.
Pour les laboratoire du cours LOG680.

Pour utiiliser l'application il faut compiler et lancer le projet avec Visual Studio ou Rider.
Pour exécuter une migration dans la base de données, veuillez vous référer à la section Base de données

# Labo 1

## Création d’un projet et du tableau Kanban dans GitHub


## Création des étiquettes


## Ajout de modèles 


## Création des milestones


## Politiques de branches


## Création de l’application
Nous avons opté pour faire un API en utilisant le framework .Net. Il s'agit d'un framework pour le développement entre autre d'applications web ou API, codés avec le langage de programmation C#. Nous utilisons aussi Visual Studio ou Rider pour compiler et exécuter le projet.

## Métriques Kanban


## Métriques “pull-request”


## Base de données
Une base de données Postgres est utilisé. Cette base de données a été fournie par le chargé de laboratoire, et nous ne faisons que l'exploiter. Une connection a été établie avec notre application en utilisant les différentes librairies que Postgres offre dans le framework .Net, puis nous utilisons l'ORM Entity Framework dedié à Postgres pour le mapping de nos objets de l'application avec les différentes tables de notre base de données. Nous pouvons aussi visualiser notre base de données dans l'application pgAdmin pour avoir une vision plus claire des tables et leur contenu.

Aussi, pour effectuer une migration, il faut d'abord créer le DbSet des items à ajouter dans la BD dans le DbContext, puis nous exécutons sur un terminal dans le projet la commande ```dotnet ef migrations add <nameForYourMigration>```, et l'appliquer en utilisant la commande ```dotnet ef database update```

## Métrique de visualisation
Les métriques de visualisation comportent le nombre d'issues qui comporte chaque colonne du Kanban, le nombre total d'issues dans le Kanban et l'heure à laquelle le Snapshot a été créé. Les métriques sur la visualisation sont pour le moment visibles dans la base de données. Une option pour accéder à ces données à partir de l'API est à prévoir.

## Tests et démonstration 
Nous avons utilisé le framework de tests NUnit, ainsi que la librairie Moq pour "moquer" des objets pour nos cas d'unit testing. Donc, nous faisons des tests unitaires pour certaines classes ayant un faible couplage, et des tests d'intégration pour celles auquel il est nécessaire d'intégrer plusieurs classes dans les tests. Par exemple, il est très difficile de faire des unit tests à certaines classes comme les controlleurs, et donc, un test d'intégration est nécessaire dans ce cas.

