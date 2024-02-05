# metrics-h24-grp2-eq2.
Pour les laboratoire du cours LOG680.

Pour utiiliser l'application il faut compiler et lancer le projet avec Visual Studio ou Rider.
Pour exécuter une migration dans la base de données, veuillez vous référer à la section Base de données

# Labo 1

## Création d’un projet et du tableau Kanban dans GitHub
Pour notre projet Kanban, nous avons créé un total de 5 colonne pour bien séparer les issues
- La colonne *Backlog* sert à tout ce qui doit être fait mais qui n'est pas prévu pour la semaine de travail en cours
- La colonne *À faire* sert à tout les éléément qui sont prévu pour la semaine en cours ou qui devrait déja avoir été rééalisé et sont en retard
- La colonne *En cours* sert à noter tout le travail qui est en cours d'accomplissement, pour chaque membre de l'équipe, il ne devrait pas avoir plus d'un ou deux élément assigné dans cette colonne
- La colonne *Revue* sert à tout le travail qui a été accompli mais qui n'a pas été revue par un second memebre de l'équipe, nous demandons une revue pour tout les changement au code de l'application, mais non obligatoire au changement de documentation
- Finalement, la colonne *Terminéé* sert de banque pour tout le travail accompli qui fait maintenant pars du projet, c'est la dernière étape de dééveloppement, lorsque la pull-request a été accepté ou la documentation écrite

### Automatisation
Nous avons mis en place un certain degré d'automatisation du Kanban avec les règles suivante:
-Lorsqu'un issue est créé, un élément lié à l'issue est créé dans le Kanban et est placé sous la colonne *Backlog*
-Lorsq'une pull request est accepté et lié à un issue, l'issue en question est ferméé et son élément dans le kanban est mis sous la colonne *Terminé*

## Création des étiquettes
Pour les étiquettes nous avons créer une liste d'étiquette assez générique qui pourrait servir dans d'autre projet aussi. Voici la lsite:
- *bug*
- *documentation*
- *duplicate*
- *enhancement*
- *Epic*
- *help wanted*
- *invalid*
- *question*
- *Rapport*
- *wontfix*

Ces étiquettes servent à aider la visibilité lors de la recherche d'élément et nous laissons à la discrétion des développeur des ajouté ou retirer aux besoins.

## Ajout de modèles 
Nous avons créer deux modèles pour la création d'issues et un modèle pour la création des template.

### Issues
- Bug report : sert a documenter un bug ou une erreur logique.
- feature request : sert à documenter une fonctionnalité qui devra être développé.

### pull request
Notre modèle de pull request permet à la revue d'avoire toute l'information nécéssaire afin de ne rien oublier.

Les modèles peuvent être accêdé ici:
Pour les issues : [bug_report.md](.github/ISSUE_TEMPLATE/bug_report.md) et [feature_request.md](.github/ISSUE_TEMPLATE/feature_request.md)
Pour les pull request: [pull_request_template.md](docs/pull_request_template.md)

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
Nous avons opté pour faire un API en utilisant le framework .Net. Il s'agit d'un framework pour le développement entre autre d'applications web ou API, codés avec le langage de programmation C#. Nous utilisons aussi Visual Studio ou Rider pour compiler et exécuter le projet. Le projet utilise la dernière version de .Net, soit .Net 8, puis on utilise avec C# 12. Ce framework comporte aussi une panoplie de librairies lesquelles on peut ajouter facilement au projet, nous permettant d'avoir un produit assez flexible et compatible avec plusieurs techonologies. Par exemple, grâce à cet atout, on peut facilement télécharger une librairie nous facilitent l'accès de connection à une base de données Postgresql, comme c'est le cas de notre projet, ce qui est mentionné dans la section Base de données.

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
Il y a 5 différentes métriques pour les pull-request:
### Lead time pour les pull-request
Cette métrique est une soustraction du temps de fermeture d'une pull-request avec le temps d'ouverture de cette même pull-request. Cela permet d'avoir le nombre de temps en jours et en heure du temps que cette pull-request a passer ouverte avant d'être fermé

### Merged time pour les pull-request
Cette métrique est semblable au Lead time mais compare le temps d'ouverture de la pull-request avec le moment ou elle a été "merge" avec dans une autre branche. Une pull-request qui a été merge a été accepté tandis qu'il est possible de fermé une pull-request en la refusant sans accepté ses modifications

### La taille d'une pull-request
Cette métrique nous permet de connaitre la taille d'une pul-request. Pour obtenir cette taille, on fait l'addition du nombre de ligne ajouté au nombres de ligne de retiré.

### Flow ratio d'un repository
Cette métrique ne vise pas spécifiquement une pull-request, masi tout les pull-request des 30 derniers jours du repository. Cette métrique est le ratio des pull-request ouvertes par rapport aux pull request fermées. Le but étant un ratio de 1, cette métrique nous montre la balance des modifications écrite sur le projet par rapport aux modifications "accepté" ou déployé

### Métrique sur les discussions
Cettre métriques est la sommes de toute les discussions et communications qui on été fait sur une pull-request. Ceci comprends les commentaire, les reviews et les review requests. Avec cette métriques, il est possible d'observé l'efficacité des discussions dans les pull-requests.


## Base de données
Une base de données Postgres est utilisé. Cette base de données a été fournie par le chargé de laboratoire, et nous ne faisons que l'exploiter. Une connection a été établie avec notre application en utilisant les différentes librairies que Postgres offre dans le framework .Net, puis nous utilisons l'ORM Entity Framework dedié à Postgres pour le mapping de nos objets de l'application avec les différentes tables de notre base de données. Nous pouvons aussi visualiser notre base de données dans l'application pgAdmin pour avoir une vision plus claire des tables et leur contenu.

Aussi, pour effectuer une migration, il faut d'abord créer le DbSet des items à ajouter dans la BD dans le DbContext, puis nous exécutons sur un terminal dans le projet la commande ```dotnet ef migrations add <nameForYourMigration>```, et l'appliquer en utilisant la commande ```dotnet ef database update```

## Métriques de visualisation
Les métriques de visualisation se base sur la prise de Snapshots. Ces snapshots sont une photographie de l'état du Kanban à un moment donnée, comportant le nom des colonnes, le nombre d'issues que chaque colonne comportait, le nombre total d'issues que le projet avait en ce moment, le nom du projet, l'ID du projet, le repository et le propriétaire, s'il y a lieu, auquel ce projet appartient, puis la date de quand le snapshot a été prise. Ces snapshots sont ensuite sauvegardés dans une base de données. Cela nous permet de garder trace de l'état du Kanban, et aussi utiliser d'utiliser ces mêmes données pour analyser d'autres métriques avec le temps.

### Obtenir les snapshots à une date
Il est possible de chercher tous les snapshots qui ont été pris à une date spécifique, s'il y a des snapshots à cette date. On peut chercher les snapshots d'un projet spécifique ou d'un repository en spéficique, tant qu'ils aient été sauvegardés à partir de notre API.

### Métrique sur la moyenne de tâches entre deux dates
Il est possible de calculer le nombre de tâches en moyenne entre deux dates. On obtient tous les snapshots entre les deux dates demandées, puis on calcule le nombre total de tâches de tous les snapshots divisé par le nombre de snapshots recueillis, puis on retourne la moyenne. En théorie, on possède un snapshot par jour, et donc la métrique de la moyenne est une donnée fiable qui peut nous aider à améliorer notre processus de travail.

### Métrique sur le goulot d'étranglement du projet
Il est possible de trouver quel est la colonne qui est pour notre projet, le goulot d'étranglement. On obtient cette métrique en obtenant tous les snapshots sauvegardés, on additionne chaque colonne avec ces données de tous les snapshots, puis on regarde quelle est la colonne qui a le plus grand nombre de tâches. On retourne après le pourcentage de ces tâches par rapport au total de tâches.

### Métrique sur le goulot d'étranglement du projet entre deux dates
C'est la même métrique que 'Métrique sur le goulot d'étranglement du projet', sauf qu'on peut effectuer la même opération entre deux dates, au lieu de tous les snapshots.


## Tests et démonstration 
Nous avons utilisé le framework de tests NUnit, ainsi que la librairie Moq pour "moquer" des objets pour nos cas d'unit testing. Donc, nous faisons des tests unitaires pour certaines classes ayant un faible couplage, et des tests d'intégration pour celles auquel il est nécessaire d'intégrer plusieurs classes dans les tests. Par exemple, il est très difficile de faire des unit tests à certaines classes comme les controlleurs, et donc, un test d'intégration est nécessaire dans ce cas.

