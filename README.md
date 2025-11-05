# BRASSART_GD2_Unity_RollABall

Objectif initial du projet

L’objectif initial du projet était de réaliser un jeu dans lequel le joueur contrôlerait une plateforme sur laquelle se trouve une balle. Celle-ci devait rouler lorsque la plateforme s’inclinait. Le joueur aurait ainsi dû faire preuve d’adresse pour faire parcourir un chemin à la balle.
Le jeu devait également inclure :
un système de cartes d’accès à récupérer dans le niveau,
des zones de tremplin permettant à la balle de sauter par-dessus des obstacles.


Début du développement

Au début du développement, je me suis concentré sur la création de la plateforme ("player").
Problème rencontré : le script ne fonctionnait pas, la plateforme ne bougeait pas.
L’idée de base était qu’une balle se déplace dans un monde fermé, en récupérant des items pour ouvrir des portes.
J’ai géré plusieurs paramètres :
les limites d’inclinaison (maximum et minimum),
la vitesse,
la lecture des inputs (GetInput();) et leur application (ApplyTilt();),
le calcul du sens d’inclinaison (float tiltX = _verticalInput * _maxTiltAngle;),
la conversion des angles en rotation (_targetRotation = Quaternion.Euler(tiltX, 0f, tiltZ);).

Ce système me semblait plus adapté que l’utilisation directe de Vector3.
Cependant, malgré ces essais, la plateforme ne bougeait toujours pas. Après avoir perdu plusieurs heures à chercher le problème, j’ai finalement décidé de partir sur une approche plus simple : le joueur contrôle directement la balle.


Exécution du projet

J’ai commencé par reprendre le code vu en cours pour le contrôle de la balle.
Ensuite, j’ai ajusté la caméra pour créer un effet de zoom (qui, après les phases de test, ne fonctionnait plus correctement) et mis en place un système de points positifs et négatifs afin de créer un premier système de game over.

Je me suis ensuite attaqué aux cartes d’accès et à la porte qui s’ouvre lorsqu’on possède la bonne carte.
Pour cela :

J’ai créé un système d’inventaire qui stocke les cartes d’accès lorsqu’elles sont récupérées.
La carte tourne sur elle-même pour attirer l’œil :
transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
Lorsqu’un objet entre dans son trigger, la carte est ajoutée à l’inventaire puis détruite.
Pour la porte d’accès, elle détecte si l’objet entrant possède le tag "Player" et la carte correspondante. Si c’est le cas, elle consomme la carte de l’inventaire et disparaît.

J’ai également ajouté :
un système de saut avec la touche Espace, en vérifiant que la balle est bien au sol grâce à un raycast, avant d’appliquer une force verticale ;
une force de gravité supplémentaire pour rendre la chute plus rapide et réaliste ;
une zone de boost, qui augmente temporairement la vitesse et la hauteur de saut de la balle.
Pour cela, je stocke les valeurs avant le boost, je les multiplie, puis je les réinitialise après un certain temps.

J’ai mis en place un système de déplacement horizontal des cibles (targets) ainsi qu’un système de points positifs et négatifs.
Pour cela, j’ai créé un Target Manager qui gère l’ensemble des targets, leurs points de spawn et de respawn. Ce système permet d’organiser le comportement global des cibles et de centraliser leur gestion.

J’ai également ajouté une plaque de pression qui, lorsqu’elle est activée, fait disparaître un mur de sa liste.
Le mur concerné est contrôlé par un script dédié, qui gère sa disparition et sa réapparition en fonction de l’état de la plaque.

J’ai intégré des zones de dialogue où le joueur déclenche l’apparition de messages à l’écran.
Ces zones sont conçues pour éviter que plusieurs dialogues ne se superposent.
Elles gèrent :
le déclenchement lors du passage du joueur,
le délai entre chaque message,
la désactivation automatique après lecture, afin d’empêcher qu’un même message n’apparaisse deux fois.

Une zone de mort (DeathBox) détecte la chute du joueur hors de la carte.
Le joueur est alors téléporté instantanément à un point de respawn (RespawnBox).
Les vitesses du Rigidbody sont réinitialisées pour éviter tout comportement anormal.
Ce système assure une reprise fluide du jeu après chaque chute.

La caméra suit le joueur avec un effet de smoothing pour un mouvement fluide.
Un offset configurable permet d’ajuster sa position.
Elle s’oriente automatiquement vers le joueur.

Le HUD affiche en temps réel le score et le nombre de cartes d’accès collectées.
Deux panneaux de fin de partie sont présents :
Victoire : s’affiche à la fin du jeu
Game Over : s’affiche à un score ≤ -1.
Chaque écran propose des boutons "Redémarrer" et "Quitter", et met le jeu en pause (Time.timeScale = 0).

Le menu principal permet de naviguer entre plusieurs panels :
Accueil avec bouton "Jouer",
Options pour les réglages,
et Quitter, fonctionnel dans l’éditeur et le build. 

