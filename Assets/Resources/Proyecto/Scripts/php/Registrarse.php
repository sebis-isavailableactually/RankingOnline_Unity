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

		// si hay un usuario con ese nombre y contraseña, no se registra
		if (mysqli_num_rows($sqlShowuser) != 0){
			echo "_error: usuario existente";
		}

		// en caso contrario, se registra insertando una nueva fila en la tabla con los datos del usuario
		else{
			echo "usuario registrado";
			$sqlInsert = "INSERT INTO $table (Contraseña, Usuario, Skin, Score)
								 VALUES ('$User_Password', '$User_Name', '$Skin_Used', $Score)";
			$insert = mysqli_query($conn ,$sqlInsert);
		}
	}
?>

<!DOCTYPE html>
<html>
    <body>
		<?php
		// esta parte solo muestra en el explorador las variables resividas. No es obligatoria

			echo "</br></br>Mis datos ";
			echo "</br> usuario: " . $User_Name;
			echo "</br> contraseña: " . $User_Password;
			echo "</br> skin: " . $Skin_Used;
			echo "</br> score: " . $Score;
		?>
    </body>
</html>