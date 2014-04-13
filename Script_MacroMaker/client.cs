// [*] Changing bricks
// [*] Odd sized bricks
// [*] Buying additional bricks
// [?] Relative orientation
// work out which way you are when making
// rotate shifting relative
// rotate rotating?!??
// find highlight colour, pref to decide if nothing, only 1st brick or all is highlighted

function execMM(){exec("Add-Ons/Script_MacroMaker/client.cs");}

$MacroMaker::Highlight = 2;
$MacroMaker::RayLength = 5;

if(!$MacroMakerMapped)
{
	$remapDivision[$remapCount] = "Macro Maker";
	$remapName[$remapCount] = "Create Macro";
	$remapCmd[$remapCount] = "makeMacroRay";
	$remapCount++;
	$MacroMakerMapped = 1;
}

function makeMacroRay(%val)
{
	if(!%val)
	{
		return;
	}
	%player = serverCommection.getControlObject();
	if(!isObject(%player))
	{
		return;
	}
	%start = %player.getEyePoint();
	%end = vectorAdd(%start, vectorMul(%player.getEyeVector(), $MacroMaker::RayLength));
	%brick = brickRayCast(%start, %end);
	if(isObject(%brick))
	{
		makeMacro(%brick);
	}
}

function brickRayCast(%start, %end)
{
	for(%i = 0; %i < 3; %i++)
	{
		%s[%i] = getWord(%start, %i);
		%e[%i] = getWord(%end, %i);
		if(%s[%i] < %e[%i])
		{
			%min[%i] = %s[%i];
			%max[%i] = %e[%i];
		}
		else
		{
			%min[%i] = %e[%i];
			%max[%i] = %s[%i];
		}
		%minCorner[%i] = mFloatLength(%min[%i] * 2, 0);
		%maxCorner[%i] = mFloatLength(%max[%i] * 2, 0);
	}
	for(%i = 0; %i < serverConnection.getCount(); %i++)
	{
		%obj = serverConnection.getObject(%i);
		if(%obj.getDataBlock() !$= "fxDtsBrick")
		{
			continue;
		}
		%worldBox = %obj.getWorldBox();
		%outside = 0;
		for(%a = 0; %a < 3; %a++)
		{
			%objMin[%a] = getWord(%worldBox, %a);
			%objMax[%a] = getWord(%worldBox, %a + 3);
			%objSize[%a] = mFloatLength(%objMax[%a] - %objMin[%a]) * 2, 0);
			if(%objMin[%a] > %max[%a] || %objMax[%a] < %min[%a])
			{
				%outside = 1;
				break;
			}
		}
		if(%outside)
		{
			continue;
		}
		for(%dim[0] = 0; %dim[0] < %objSize[0]; %dim[0]++)
		{
			for(%dim[1] = 0; %dim[1] < %objSize[1]; %dim[1]++)
			{
				for(%dim[2] = 0; %dim[2] < %objSize[2]; %dim[2]++)
				{
					%skip = 0;
					for(%b = 0; %b < 3; %b++)
					{
						%pos[%b] = mFloatLength(%objMin[%b] * 2, 0) + %dim[%b];
						if(%pos[%b] < %minCorner[%b] || %pos[%b] > %maxCorner[%b])
						{
							%skip = 1;
							break;
						}
					}
					if(%skip)
					{
						continue; // maybe skip to a certain position in the loops based on which dimension fails
					}
					%grid[%pos[0] - minCorner[0], %pos[1] - minCorner[1], %pos[2] - minCorner[2]] = %obj;
				}
			}
		}
	}
	
	// make an array of all bricks dont leur world box is within the box with ray as a diagonal
	// using this list, make a 3d array of each possible 1x1f in this box, with the id of the brick which occupies that certain 1x1f
	// loop through each of the 1x1f's that the ray intersects, choose the first which is an object

	// %obj.getWorldBox(); returns two positions, the closest pos to origin and furthest
	// to get a worldBox from %start and %end, take the lower val for x/y/z as first point, then higher val for x/y/z as second point
}

// maybe send in %start and %end into these functions where %something is
// or something precalculated in the main function to avoid lots of recalculations

function getPosFromX(%something, %x)
{
	
}

function getPosFromY(%something, %y)
{
	
}

function getPosFromZ(%something, %z)
{
	
}

function determineBrightestColor()
{
	%target = "1 1 1 1";
	for(%i = 0; %i < 64; %i++)
	{
		%colori = getColorIdTable(%i);
		%dist = vectorDist(%colori, %target);
		if(%dist < %best || %best $= "")
		{
			%best = %dist;
			$MacroMaker::HighlightColor = %i;
		}
	}
}

function makeMacro(%brick)
{
	if(!isObject(%brick) || %brick.getClassName() !$= "fxDtsBrick")
	{
		messageBoxOK("MacroMaker Error", "You must select a brick to create a Macro.");
		return;
	}
	if(isObject($BuildMacroSO))
		$BuildMacroSO.delete();
	$BuildMacroSO = new ScriptObject()
	{
		class = "BuildMacroSO";
		numEvents = 2;
		event0 = "Server\tUseSprayCan\t" @ %brick.getColorID();
		event1 = "Server\tPlantBrick";
		brickArray = %brick.getDataBlock();
		brickRot = %brick.getAngleID();
		brickPos = %brick.getPosition();
		brickColor = %brick.getColorID();
		brickData = %brick.getDataBlock();
	};
	
}

function makeMacro(%brick)
{
	if(!isObject(%brick) || %brick.getClassName() !$= "fxDtsBrick")
	{
		messageBoxOK("MacroMaker Error", "You must select a brick to create a Macro.");
		return;
	}
	if(isObject($BuildMacroSO))
		$BuildMacroSO.delete();
	$BuildMacroSO = new ScriptObject()
	{
		class = "BuildMacroSO";
		numEvents = 2;
		event0 = "Server\tUseSprayCan\t" @ %brick.getColorID();
		event1 = "Server\tPlantBrick";
		brickArray = %brick.getDataBlock();
		brickRot = %brick.getAngleID();
		brickPos = %brick.getPosition();
		brickColor = %brick.getColorID();
		brickData = %brick.getDataBlock();
	};
	%brick.inMacro = 1;
	for(%i=0;%i<%brick.getNumUpBricks();%i++)
	{
		%nextBrick = %brick.getUpBrick(%i);
		if(!%nextBrick.inMacro)
			macroAdd(%nextBrick);
	}
	for(%i=0;%i<ServerConnection.getCount();%i++)
	{
		%obj = ServerConnection.getObject(%i);
		if(%obj.inMacro)
		{
			%obj.inMacro = "";
			%num++;
		}
	}
	$BuildMacroSO.brickRot = "";
	$BuildMacroSO.brickPos = "";
	$BuildMacroSO.brickColor = "";
	$BuildMacroSO.brickData = "";
	for(%i=0;%i<10;%i++)
		$BSD_InvData[%i] = getField($BuildMacroSO.brickArray, %i);
	BSD_BuyBricks();
	UseFirstSlot(1);
	%s = (%num > 1) ? "s" : "";
	clientCmdCenterPrint("Macro Created: " @ %num @ " brick" @ %s @ ".", 2, 2);
}

function macroAdd(%brick)
{
	%macro = $BuildMacroSO;
	%brick.inMacro = 1;
	%datablock = %brick.getDataBlock();
	if(%brick.getDataBlock() != %macro.brickData)
	{
		//for(%i=0;%i<getFieldCount($MacroExtraBrickArray);%i++)
		//{
		//	%extraBrick = getField($MacroExtraBrickArray, %i);
		//	if(%extraBrick == %brick.getDataBlock())
		//	{
		//		%slot = %extraBrick;
		//		break;
		//	}
		//}
		//if(!%slot)
		//{
		//	$MacroExtraBricks++;
		//	if($MacroExtraBricks >= 10)
		//		$MacroExtraBricks = 0;
		//	%macro.AddEvent("Server\tBuyBrick\t" @ $MacroExtraBricks TAB %brick.getDataBlock());
		//	%slot = $MacroExtraBricks;
		//}
		%fields = getFieldCount(%macro.brickArray);
		%slot = -1;
		for(%i=0;%i<%fields;%i++)
		{
			if(getField(%macro.brickArray, %i) == %datablock)
			{
				%slot = %i;
				break;
			}
		}
		if(%slot < 0)
		{
			if(%fields >= 10)
			{
				// buy bricks or something
			}
			else
			{
				%slot = %fields;
				%macro.brickArray = %macro.brickArray TAB %datablock;
			}
		}
		%macro.addEvent("Server\tUseInventory\t" @ %slot);
		%macro.brickData = %brick.getDataBlock();
	}
	%color = %brick.getColorID();
	if(%color != %macro.brickColor)
	{
		%macro.addEvent("Server\tUseSprayCan\t" @ %color);
		%macro.brickColor = %color;
	}
	%angle = %brick.getAngleID();
	if(%angle != %macro.brickRot)
	{
		%change = %angle - %macro.brickRot;
		if(%change < -2)
			%macro.addEvent("Server\tRotateBrick\t1");
		else if(%change > 2)
			%macro.addEvent("Server\tRotateBrick\t-1");
		else
		{
			if(%change > 0)
			{
				while(%change > 0)
				{
					%macro.addEvent("Server\tRotateBrick\t1");
					%change--;
				}
			}
			else
			{
				while(%change < 0)
				{
					%macro.addEvent("Server\tRotateBrick\t-1");
					%change++;
				}
			}
		}
		%macro.brickRot = %angle;
	}
	//%tempbrick = new fxDtsBrick(){
	//	datablock = %brick.getdatablock();
	//};
	//%tempbrick.setTransform($MacroPos SPC getwords(%brick.getTransform(), 3, 6));
	//$MacroPos = %tempbrick.getPosition();
	//%tempbrick.delete();
	%position = %brick.getPosition();
	%poschange = vectorSub(%position, %macro.brickPos);
	%x = mFloatLength(getWord(%poschange, 0) * 2, 0);
	%y = mFloatLength(getWord(%poschange, 1) * 2, 0);
	%z = mFloatLength(getWord(%poschange, 2) * 5, 0);
	%macro.brickPos = %position;
	%macro.addEvent("Server\tShiftBrick\t" @ %x TAB %y TAB %z);
	%macro.addEvent("Server\tPlantBrick");
	for(%i=0;%i<%brick.getNumUpBricks();%i++)
	{
		%nextBrick = %brick.getUpBrick(%i);
		if(!%nextBrick.inMacro)
			macroAdd(%nextBrick);
	}
	for(%i=0;%i<%brick.getNumDownBricks();%i++)
	{
		%nextBrick = %brick.getDownBrick(%i);
		if(!%nextBrick.inMacro)
			macroAdd(%nextBrick);
	}
}

function getPlayerRot()
{
	%player = ServerConnection.getControlObject();
	return getWord(%player.getTransform(), 6) / 90;
}

function findNearestBrick()
{
	%player = ServerConnection.getControlObject();
	if(!isObject(%player))
		return;
	%position = %player.getPosition();
	%distance = 100;
	for(%i=0;%i<ServerConnection.getCount();%i++)
	{
		%brick = ServerConnection.getObject(%i);
		if(%brick.getClassName() !$= "fxDtsBrick")
			continue;
		%dist = VectorDist(%brick.getPosition(), %position);
		if(%dist < %distance)
		{
			%distance = %dist;
			%nearest = %brick;
		}
	}
	if(isObject(%nearest))
	{
		%nearest.schedule(1000, "setColor", %nearest.colorID);
		%nearest.setColor(4);
		echo(%nearest);
		return %nearest;
	}
	error("No bricks found within 100 units radius");
}