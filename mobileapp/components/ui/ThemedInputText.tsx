import React from 'react';
import { StyleSheet, TouchableOpacity, TouchableOpacityProps, ViewStyle, TextStyle } from 'react-native';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';

interface ButtonProps extends TouchableOpacityProps {
	title: string;
	onPress: () => void;
	style?: ViewStyle;
	textStyle?: TextStyle;
}

const Button: React.FC<ButtonProps> = ({ title, onPress, style, textStyle, ...rest }) => {
	return (
		<TouchableOpacity onPress={onPress} style={[styles.container, style]} {...rest}>
			<ThemedView style={styles.innerContainer}>
				<ThemedText style={[styles.text, textStyle]}>{title}</ThemedText>
			</ThemedView>
		</TouchableOpacity>
	);
};

const styles = StyleSheet.create({
	container: {
		backgroundColor: '#6200EE',
		borderRadius: 8,
		paddingVertical: 10,
		paddingHorizontal: 20,
	},
	innerContainer: {
		justifyContent: 'center',
		alignItems: 'center',
	},
	text: {
		color: '#FFFFFF',
		fontSize: 16,
		fontWeight: 'bold',
	},
});

export default Button;
