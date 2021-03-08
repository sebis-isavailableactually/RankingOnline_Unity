<?php
	// link de la web donde esta el servidor
	$server = "";

	// nombre del usuario para acceder a la base de datos
	$user = "";

	// contraseña para acceder a la base de datos
	$password = "";

	// nombre de la base de datos a acceder
	$database = "";

	// nombre de la tabla a acceder
	$table = "";

    $conn = mysqli_connect($server, $user, $password , $database);

	// variables que se tomaran
	$User_Password = $_GET["password"];
	$User_Name = $_GET["name"];
	$Skin_Used = $_GET["skin"];
	$Score = $_GET["score"];
		//?name=&password=&score=&skin=

    if (!$conn){
		// no se puedo conectar a la base de datos
        echo "error de conexión";
	}
	else{
		// se pudo conectar a la base de datos...
		// busca en toda la tabla si hay un usuario o contraseña iguales
		$sqlcheckuser = "SELECT * FROM $table WHERE Usuario LIKE '$User_Name' AND Contraseña LIKE '$User_Password'";
		$sqlShowuser = mysqli_query($conn, $sqlcheckuser);

		// si no encuentra al usuario, manda un mensaje de error
		if (mysqli_num_rows($sqlShowuser) == 0){
			echo "_error en los datos";
		}

		// en caso contrario, si lo encuentra...
		else{
			echo "usuario encontrado. ";

			// si lo encuentra, revisa si la puntuacion es mayor a la que esta registrada en la base de datos (rompio su record?)
			$sqlRead = "SELECT Score FROM $table WHERE Usuario='$User_Name'";
			$Find = mysqli_query($conn, $sqlRead);
			$Value = mysqli_fetch_array($Find);
	
			// si rompio su record, se actualizan los datos de este usuario
			if ($Value[0] < $Score){
				$sqlrequestupdate = "UPDATE ranking SET Score = '$Score', Skin = '$Skin_Used'
				WHERE Usuario = '$User_Name' AND Contraseña = '$User_Password'";

				$sqlupdate = mysqli_query($conn, $sqlrequestupdate);
			}
			
			// si no lo rompio, no actualiza al usuario en la base de datos
			else{
				echo "no supero la puntuación maxima";
			}
		}
	}
?>

<!DOCTYPE html>
<html>
    <body>
		<?php
		// esta parte solo muestra en el explorador las variables resividas. No es obligatoria

			echo "</br></br>Datos erroneos ";
			echo "</br> user: " . $User_Name;
			echo "</br> password: " . $User_Password;
			echo "</br> skin: " . $Skin_Used;
			echo "</br> score: " . $Score;
		?>
    </body>
</html>