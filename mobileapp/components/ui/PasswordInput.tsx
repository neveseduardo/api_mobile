import React, { useState } from 'react';
import { TextInput as RNTextInput, TextInputProps, View, TouchableOpacity, Text } from 'react-native';
import clsx from 'clsx';
import Ionicons from '@expo/vector-icons/Ionicons';

interface PasswordInputProps extends TextInputProps {
	placeholder?: string;
	value?: string;
	onChangeText?: (text: string) => void;
	className?: string;
	error?: boolean;
	errorMessage?: string;
}

const PasswordInput = ({
	placeholder,
	value,
	onChangeText,
	className,
	error = false,
	errorMessage,
	style,
	...rest
}: PasswordInputProps) => {
	const [showPassword, setShowPassword] = useState(false);

	const togglePasswordVisibility = () => {
		setShowPassword((prev) => !prev); // Alterna o estado de showPassword
	};

	return (
		<View className="w-full">
			<View
				className={clsx(
					'w-full flex-row items-center h-[40px] bg-white border border-gray-300 rounded focus:border-blue-500 dark:bg-gray-800 dark:border-gray-600',
					error && 'border-red-500',
					className
				)}
			>
				<RNTextInput
					autoCorrect={false}
					autoComplete="off"
					placeholder={placeholder}
					value={value}
					onChangeText={onChangeText}
					secureTextEntry={!showPassword} // Oculta o texto se showPassword for false
					className={clsx(
						'flex-1 h-full px-4 text-black dark:text-white placeholder-gray-400 dark:placeholder-gray-500'
					)}
					style={style}
					{...rest}
				/>
				<TouchableOpacity onPress={togglePasswordVisibility} className="mr-2">
					<Ionicons
						name={showPassword ? 'eye-off' : 'eye'} // Alterna entre os ícones de olho aberto e fechado
						size={24}
						color="#6B7280"
					/>
				</TouchableOpacity>
			</View>
			{error && errorMessage && (
				<Text className="mt-1 text-sm text-red-500">{errorMessage}</Text>
			)}
		</View>
	);
};

export default PasswordInput;