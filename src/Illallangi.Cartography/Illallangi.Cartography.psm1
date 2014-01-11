if ($null -eq (Get-Module PSCompletion))
{
	Write-Debug "Import-Module PSCompletion -Global"
	Import-Module PSCompletion -Global -ErrorAction SilentlyContinue
	if ($null -eq (Get-Module PSCompletion))
	{
		Write-Warning "PSCompletion module not found; tab completion will be unavailable."
	}
}

if ($null -ne (Get-Module PSCompletion))
{
	Register-ParameterCompleter New-Map OutputFormat {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		"Jpeg", "Bmp", "Gif", "Png", "Tiff" | Where-Object { $_ -like "$($wordToComplete)*" } | Sort-Object { $_ } |%{ New-CompletionResult """$($_)""" }
	}
}

Function Get-GreatCircleFlights
{
	foreach ($flight in Get-Flight)
	{
		Write-Debug "$($flight.Origin) to $($flight.Destination)"
		$origin = (Get-Airport | Where-Object { $_.Icao -eq $flight.Origin})
		if (($null -eq $origin) -or ($origin.Count > 1))
		{
			Write-Error "Unable to source $($flight.Origin)"
			Exit
		}

		$destination = (Get-Airport | Where-Object { $_.Icao -eq $flight.Destination})
		if (($null -eq $destination) -or ($destination.Count > 1))
		{
			Write-Error "Unable to source $($flight.Destination)"
			Exit
		}

		New-Object –TypeName PSObject |
				Add-Member –MemberType NoteProperty –Name OriginLatitude –Value $origin.Latitude –PassThru |
				Add-Member –MemberType NoteProperty –Name OriginLongitude –Value $origin.Longitude –PassThru |
				Add-Member –MemberType NoteProperty –Name DestinationLatitude –Value $destination.Latitude -PassThru |
				Add-Member -MemberType NoteProperty -Name DestinationLongitude -Value $destination.Longitude -PassThru
	}
}