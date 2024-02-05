# metrics-h24-grp2-eq2.
Pour les laboratoire du cours LOG680.

Pour utiiliser l'application il faut compiler et lancer le projet avec Visual Studio ou Rider.
Pour exécuter une migration dans la base de données, veuillez vous référer à la section Base de données

# Labo 1

## Création d’un projet et du tableau Kanban dans GitHub


## Création des étiquettes


## Ajout de modèles 


## Création des milestones
Il y a 3 Milestones pour notre projet: 
- *Labo 1*
- *Labo 2*
- *Labo 3*

Ces milestones nous permettent de savoir pour quelle laboratoire chaque "Issue" a été créee et donc maintenir un ordre dans le tableau Kanban. 


## Politiques de branches
Pour ce projet, nous avons utilisé la méthodologie GitHub Flow afin de rationaliser notre processus de développement. Voici une brève explication de ce que cela implique :

### Mise en œuvre de GitHub Flow :
#### Branchement :
- Chaque fonctionnalité ou correction de bogue commence par la création d'une nouvelle branche.
- Les branches maintiennent les modifications isolées jusqu'à ce qu'elles soient prêtes à être intégrées.
#### Commits :
- Les développeurs effectuent de petits changements incrémentiels avec des commits réguliers sur la branche.
- Cette pratique facilite la collaboration et fournit une chronologie claire du développement.
#### Demandes de fusion (PR) :
- Lorsqu'une fonctionnalité ou une correction est terminée, une demande de fusion est lancée.
- Les PR servent de points de discussion pour la revue de code, les retours et les tests.
#### Fusion :
- Après approbation, la branche est fusionnée dans la base de code principale.
- Cela garantit une intégration en douceur sans perturber la stabilité du code existant.

## Création de l’application
Nous avons opté pour faire un API en utilisant le framework .Net. Il s'agit d'un framework pour le développement entre autre d'applications web ou API, codés avec le langage de programmation C#. Nous utilisons aussi Visual Studio ou Rider pour compiler et exécuter le projet.

## Métriques Kanban
Il y a 4 différentes métriques Kanban: 
### Lead Time pour un Issue
Cette métrique permet d'obtenir le temps pour une tâche donnée, si la tâche a été "closed", le temps est calculé avec la date de fermeture et la date de création. Cependant, si la tâche n'est pas finie, le temps est calculé entre la date de création et la date de la requête. 

### Lead Time pour les tâches finies dans une période donnée
Cette métrique obtient toutes les tâches qui ont été finies dans l'intervalle de temps choisi par l'utilisateur et calcule le temps que ces tâches ont pris de leur création à leur fermeture. 

### Nombre de tâches pour une colonne donnée
Avec cette métrique on peut obtenir le nombre de tâches qui sont actives dans une colonne du tableau Kanban, la colonne doit être choisie par l'utilisateur. Par exemple, on pourrait savoir combien de tâches sont encore dans le backlog. 

### Nombre de tâches complétées pour une période donnée
Cette métrique donne comme résultat le nombre de tâches qui ont été accomplies pendant la période donnée par l'utilisateur. On peut par exemple obtenir le nombre de tâches qui ont été finies entre le 4 janvier 2024 et le 5 février 2024. 

## Métriques “pull-request”


## Base de données
Une base de données Postgres est utilisé. Cette base de données a été fournie par le chargé de laboratoire, et nous ne faisons que l'exploiter. Une connection a été établie avec notre application en utilisant les différentes librairies que Postgres offre dans le framework .Net, puis nous utilisons l'ORM Entity Framework dedié à Postgres pour le mapping de nos objets de l'application avec les différentes tables de notre base de données. Nous pouvons aussi visualiser notre base de données dans l'application pgAdmin pour avoir une vision plus claire des tables et leur contenu.

Aussi, pour effectuer une migration, il faut d'abord créer le DbSet des items à ajouter dans la BD dans le DbContext, puis nous exécutons sur un terminal dans le projet la commande ```dotnet ef migrations add <nameForYourMigration>```, et l'appliquer en utilisant la commande ```dotnet ef database update```

## Métrique de visualisation
Les métriques de visualisation comportent le nombre d'issues qui comporte chaque colonne du Kanban, le nombre total d'issues dans le Kanban et l'heure à laquelle le Snapshot a été créé. Les métriques sur la visualisation sont pour le moment visibles dans la base de données. Une option pour accéder à ces données à partir de l'API est à prévoir.

## Tests et démonstration 
Nous avons utilisé le framework de tests NUnit, ainsi que la librairie Moq pour "moquer" des objets pour nos cas d'unit testing. Donc, nous faisons des tests unitaires pour certaines classes ayant un faible couplage, et des tests d'intégration pour celles auquel il est nécessaire d'intégrer plusieurs classes dans les tests. Par exemple, il est très difficile de faire des unit tests à certaines classes comme les controlleurs, et donc, un test d'intégration est nécessaire dans ce cas.

