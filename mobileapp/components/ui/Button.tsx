import React from 'react';
import { TouchableOpacity, TouchableOpacityProps, TextStyle, View, Text } from 'react-native';

interface ButtonProps extends TouchableOpacityProps {
	title: string;
	onPress: () => void;
	textStyle?: TextStyle;
}

const Button = ({ title, onPress, style, textStyle, ...rest }: ButtonProps) => {
	return (
		<TouchableOpacity onPress={onPress} {...rest}>
			<View className=''>
				<Text>{title}</Text>
			</View>
		</TouchableOpacity>
	);
};

export default Button;
